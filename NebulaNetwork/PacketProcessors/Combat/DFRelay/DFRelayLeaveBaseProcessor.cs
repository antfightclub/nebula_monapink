﻿#region

using System.Buffers.Text;
using NebulaAPI.Packets;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Combat.DFRelay;
using NebulaWorld;

#endregion

namespace NebulaNetwork.PacketProcessors.Combat.DFRelay;

[RegisterPacketProcessor]
public class DFRelayLeaveBaseProcessor : PacketProcessor<DFRelayLeaveBasePacket>
{
    protected override void ProcessPacket(DFRelayLeaveBasePacket packet, NebulaConnection conn)
    {
        var hiveSystem = GameMain.spaceSector.GetHiveByAstroId(packet.HiveAstroId);
        if (hiveSystem == null) return;

        hiveSystem.relayNeutralizedCounter = packet.RelayNeutralizedCounter + 1;
        var dfrelayComponent = hiveSystem.relays.buffer[packet.RelayId];
        if (dfrelayComponent == null || dfrelayComponent.id != packet.RelayId) return;

        using (Multiplayer.Session.Enemies.IsIncomingRelayRequest.On())
        {
            var astroId = dfrelayComponent.targetAstroId;
            var baseId = dfrelayComponent.baseId;
            dfrelayComponent.stage = 2;
            dfrelayComponent.LeaveBase();

            //In NotifyBaseRemoving, set all relay that target the same planet and baseId to 0
            for (var enemyDFHiveSystem = hiveSystem.firstSibling; enemyDFHiveSystem != null; enemyDFHiveSystem = enemyDFHiveSystem.nextSibling)
            {
                for (var i = 1; i < enemyDFHiveSystem.relays.cursor; i++)
                {
                    dfrelayComponent = enemyDFHiveSystem.relays.buffer[i];
                    if (dfrelayComponent != null && dfrelayComponent.targetAstroId == astroId && dfrelayComponent.baseId == baseId)
                    {
                        dfrelayComponent.baseId = 0;
                    }
                }
            }
        }
    }
}
