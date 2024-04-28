using Grpc.Core;
using Pos;
using System.Data;
using System.Diagnostics;

class ServicerImpl : PosServicer.PosServicerBase
{
    public override Task<PosReply> PosCall(PosRequest request, ServerCallContext context)
    {
        PosReply reply = new PosReply();

        //������Ϣ
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

        //������Ϣ
        if (request != null)
        {
            //�������
            if (request.Code == 0)
                reply.Code = 0;
            else//����쳣
                reply.Code = 1;
        }
        return Task.FromResult(reply);
    }
}
