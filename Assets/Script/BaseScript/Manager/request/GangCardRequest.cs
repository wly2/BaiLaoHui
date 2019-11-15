using LitJson;

namespace AssemblyCSharp
{
    public class GangCardRequest : ClientRequest
    {
        public GangCardRequest(int cardPoint, int gangType)
        {
            headCode = APIS.GANGPAI_REQUEST;
            var vo = new GangRequestVO
            {
                cardPoint = cardPoint,
                gangType = gangType
            };
            messageContent = JsonMapper.ToJson(vo);
        }
    }
}