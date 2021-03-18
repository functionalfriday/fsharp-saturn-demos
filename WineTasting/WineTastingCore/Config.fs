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

type SecretSettings = {
    GithubSecret : string
}