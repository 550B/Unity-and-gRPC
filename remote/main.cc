#include <iostream>
#include <memory>
#include <string>

#include "absl/flags/flag.h"
#include "absl/flags/parse.h"
#include "absl/strings/str_format.h"

#include <grpcpp/ext/proto_server_reflection_plugin.h>
#include <grpcpp/grpcpp.h>
#include <grpcpp/health_check_service_interface.h>

#include "pos.grpc.pb.h"

using grpc::Server;
using grpc::ServerBuilder;
using grpc::ServerContext;
using grpc::Status;
using pos::PosServicer;
using pos::EasyCallRequest;
using pos::EasyCallReply;
using pos::PosRequest;
using pos::PosReply;

ABSL_FLAG(uint16_t, port, 2333, "Server port for the service");

// Logic and data behind the server's behavior.
class PosServicerImpl final : public pos::PosServicer::Service {
public:
  Status EasyCall(ServerContext* context, const EasyCallRequest* request,
                  EasyCallReply* reply) {
    reply->set_code(0);
    return Status::OK;
  }

  Status PosCall(ServerContext* context, const PosRequest* request, PosReply* reply) {
    float x = request->x();
    float y = request->y();
    float z = request->z();

    if (x>=-5&&x<=5&&z>=-5&&z<=5&&y>=-5) {
      reply->set_id(0);
      reply->set_inside(true);
    }
    else
    {
      reply->set_id(-1);
      reply->set_inside(false);
    }
    return Status::OK;
  }
};

void RunServer(uint16_t port) {
  std::string server_address = absl::StrFormat("0.0.0.0:%d", port);
  PosServicerImpl service;

  grpc::EnableDefaultHealthCheckService(true);
  grpc::reflection::InitProtoReflectionServerBuilderPlugin();
  ServerBuilder builder;
  // Listen on the given address without any authentication mechanism.
  builder.AddListeningPort(server_address, grpc::InsecureServerCredentials());
  // Register "service" as the instance through which we'll communicate with
  // clients. In this case it corresponds to an *synchronous* service.
  builder.RegisterService(&service);
  // Finally assemble the server.
  std::unique_ptr<Server> server(builder.BuildAndStart());
  std::cout << "Server listening on " << server_address << std::endl;

  // Wait for the server to shutdown. Note that some other thread must be
  // responsible for shutting down the server for this call to ever return.
  server->Wait();
}

int main(int argc, char** argv) {
  absl::ParseCommandLine(argc, argv);
  RunServer(absl::GetFlag(FLAGS_port));
  return 0;
}

