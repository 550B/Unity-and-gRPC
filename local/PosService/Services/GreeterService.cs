using Grpc.Core;
using Pos;
using System.Data;
using System.Diagnostics;

class ServicerImpl : PosServicer.PosServicerBase
{
    public override Task<PosReply> PosCall(PosRequest request, ServerCallContext context)
    {
        PosReply reply = new PosReply();

        //处理消息
        float x = request.X;
        float y = request.Y;
        float z = request.Z;

        if (x>=-5&&x<=5&&z>=-5&&z<=5&&y>=-5)
        {
            reply.Id = 1;
            reply.Inside = true;
        }
        else
        {
            reply.Id = -1;
            reply.Inside = false;
        }

        return Task.FromResult(reply);
    }

    public override Task<EasyCallReply> EasyCall(EasyCallRequest request, ServerCallContext context)
    {
        EasyCallReply reply = new EasyCallReply();

        //处理消息
        if (request != null)
        {
            //结果正常
            if (request.Code == 0)
                reply.Code = 0;
            else//结果异常
                reply.Code = 1;
        }
        return Task.FromResult(reply);
    }
}
