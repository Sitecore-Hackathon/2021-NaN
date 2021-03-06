using ProtoBuf;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Storage
{
    [ProtoContract]
    public class Segments
    {
        [ProtoMember(1)]
        public double MonetaryMin { get; set; }
        
        [ProtoMember(2)]
        public double MonetaryMax { get; set; }

        [ProtoMember(3)]
        public double FrequencyMin { get; set; }

        [ProtoMember(4)]
        public double FrequencyMax { get; set; }

        [ProtoMember(5)]
        public double RecencyMin { get; set; }

        [ProtoMember(6)]
        public double RecencyMax { get; set; }
    }
}
