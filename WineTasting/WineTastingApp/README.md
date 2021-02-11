# Notes...

## TODOs

- maybe combine with `use_config` in `application` CE in `Program.fs`? See default Saturn template for usage example...
- remove claimsMapping?

## 2021-02-12

- We managed to move all hardcoded config values to config files. We can read these config files using [FsConfig](https://github.com/demystifyfp/FsConfig).
- We also split config files into 
  - `appsettings.json` -> git
  - `secrets.json` -> will be added to `.gitignore` in the future. This file contains secrets/credentials/etc which will not be included in git

## TODOs

- use `FsConfig` so we don't have to hardcode credentials and secrets.

## 2021-02-09

- `Program.fs`: Basic application configurations
    - some configs are hardcoded in `Config.fs`
    - `logging`
    - `force_ssl`
    - `use_static` root location of static files
    - `use_github_oauth` here is where the OAuth2 magic happens
- `Router.fs` currently includes all routes and pipelines
- Views:
    - `Index.fs`
    - `UserViews.fs` includes the "members-only" views.
      - `UserPage` should be accessible for anybody who logs in with Github
      - `AdminPage` should only be accessible for user fullName as hardcoded in `Users.matchUpUsers`  
- OAuth "logic": `Users.fs`
- hardcoded configurations: `Config.fs`

### Endpoints

Base URL: https://localhost:5000/

- [https://localhost:5000/](https://localhost:5000/) should render h1 "Hello from Saturn!"
- [https://localhost:5000/members-only](https://localhost:5000/members-only)
    - should render h1 "admin/user" once you have confirmed your Github credentials after an initial redirect
    - in case the static site path is resolved correctly, a blue background should be rendered
- [https://localhost:5000/members-only/admin](https://localhost:5000/members-only/admin)
    - should render h1 "admin/admin"
    - if you see text "Must be admin" the validation in `Users.fs` did not pass.
      Currently, for the validation to pass, the "fullName" must match the hardcoded value in `Users.matchUpUsers`.
