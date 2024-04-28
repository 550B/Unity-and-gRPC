#### 远程C++后端

#### Remote C++ backend

`protobuf`生成C++代码：

`protobuf` generates C++ code:

```sh
protoc -I ./ --cpp_out=. pos.proto
protoc -I ./ --grpc_out=. --plugin=protoc-gen-grpc=`which grpc_cpp_plugin` pos.proto
```

生成`.out`文件：

Build the `.out` using `cmake`:

```sh
$ mkdir -p cmake/build
$ pushd cmake/build
$ cmake -DCMAKE_PREFIX_PATH=$MY_INSTALL_DIR ../..
$ make -j 4
```
