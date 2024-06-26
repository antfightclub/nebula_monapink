﻿#region

using NebulaAPI.Packets;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Factory.Belt;
using NebulaWorld;

#endregion

namespace NebulaNetwork.PacketProcessors.Factory.Belt;

[RegisterPacketProcessor]
internal class BeltSignalNumberProcessor : PacketProcessor<BeltSignalNumberPacket>
{
    protected override void ProcessPacket(BeltSignalNumberPacket packet, NebulaConnection conn)
    {
        using (Multiplayer.Session.Factories.IsIncomingRequest.On())
        {
            var cargoTraffic = GameMain.galaxy.PlanetById(packet.PlanetId)?.factory?.cargoTraffic;
            if (cargoTraffic == null)
            {
                return;
            }
            cargoTraffic.SetBeltSignalNumber(packet.EntityId, packet.Number);
        }
    }
}
