
Creating a single executable

run the following command within the `WineTastingWeb` folder:

```shell
dotnet publish --runtime linux-arm --self-contained true -p:PublishTrimmed=true -p:PublishSingleFile=true -p:DebugType=None -p:Deterministic=false
```
