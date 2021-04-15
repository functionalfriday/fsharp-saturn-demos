# Notes...

## TODO

- currently the database is copied (!) from main folder to bin/Debug/net5... This is stupid -> fix

## 2021-04-15

- NOTE: the database 'app.sqlite' is located in `bin/Debug/net5.0/` !

## 2021-04-08
- convert from DB-model to ViewModel
- add Wine create form basics

## 2021-03-25

- add existing Wines table
- make Wines from Db Dapper compatible via CLIMutable

## 2021-03-18

We decided to split the project:

- WineTastingCore
- WineTastingWeb

TODO fuer naechste Session?

-> Giraffe View Engine

## 2021-03-11 "Add Wine object"

We now have a basic understanding of how

- data is loaded and stored with sqlite
- and added a 2nd 'thing' ("Wine") to the app (and the db)
- we replicated some code (don't generalize code until you've repeated it more than 3 times)

## 2021-02-18 "Database"

We started adding persistence to the application.

We did some research, and decided not to use TypeProviders, because most are not ready for linux/dotnet5.

We also decided against anything too fancy.

The following articles, although from 2017, work out-of-the-box with linux/dotnet5:

- https://www.codesuji.com/2017/07/28/F-and-SQLite/
- https://www.codesuji.com/2017/07/29/F-and-Dapper/

We also integrated the DB into the web application: 
Currently everybody who logs into the "members-only" area with Github OAuth will be stored in the Sqlite DB.

There is one improvement in our code when comparing it to the original blog post from 2017:
Since F# now has anonymous records (the `{| prop1 = value1 |}` syntax) the boxing described in the original post is not needed anymore.

And yes: the current state is very procedural, but we wanted to understand how to access a DB (here: sqlite) first...

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
