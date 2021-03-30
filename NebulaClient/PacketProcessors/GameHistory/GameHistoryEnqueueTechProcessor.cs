﻿using NebulaModel.Attributes;
using NebulaModel.Logger;
using NebulaModel.Networking;
using NebulaModel.Packets.GameHistory;
using NebulaModel.Packets.Processors;
using NebulaWorld.GameDataHistory;

namespace NebulaClient.PacketProcessors.GameHistory
{
    [RegisterPacketProcessor]
    class GameHistoryEnqueueTechProcessor : IPacketProcessor<GameHistoryEnqueueTechPacket>
    {
        public void ProcessPacket(GameHistoryEnqueueTechPacket packet, NebulaConnection conn)
        {
            Log.Info($"Enquing new tech ID: {packet.TechId}");
            GameDataHistoryManager.IsIncommingRequest = true;
            GameMain.history.EnqueueTech(packet.TechId);
            GameDataHistoryManager.IsIncommingRequest = false;
        }
    }
}
