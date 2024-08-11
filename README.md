# basic-article-api
Quick and dirty sample articles api implementation

## Opinions used:
1. A service that only calls repo methods is useless.
2. Interfaces shall arise only where multiple implementations for the same functionality are intended.
3. Failiures in validation are handled by throwing application exceptions which will be turned into bad request responses
4. Although proper password storage (hash with salt) isn't difficult, I like single sign on.
5. Organise code in the spirit of microservices (self contained verticalities).

## Design outline:
A basic CRUD built with minimal API.\
Further details on the [Wiki](https://github.com/Vikcoc/basic-article-api/wiki/Resources).
