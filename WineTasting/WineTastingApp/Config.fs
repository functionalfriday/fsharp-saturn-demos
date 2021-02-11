module Config

type LogLevel = {
    Default: string
    System : string
    Microsoft : string
}

type Logging = {
    LogLevel : LogLevel
}

type GitHub = {
    ClientId: string
    ClientSecret: string
    CallbackPath : string
}

type MyConfigs = {
    GitHub : GitHub
}

type AppSettings = {
    Url : string
    Logging : Logging
    MyConfigs : MyConfigs
}