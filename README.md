# basic-article-api
Quick and dirty sample articles api implementation

## Opinions used:
1. A service that only calls repo methods is useless.
2. Interfaces shall arise only where multiple implementations for the same functionality are intended.
3. Failiures in validation are handled by throwing application exceptions which will be turned into bad request responses
4. Although proper password storage (hash with salt) isn't difficult, I like single sign on.

## Design outline:
- Use minimal api.
- Use JWT.
- Have "Sign in with Google" endpoint (interfaced service because multiple providers for OAuth can exist).
- Have a generate token endpoint (in dev mode only) for ease of testing.
- Have reader and admin roles for crud purposes (store them in hardcoded repo because we can use them with generate token endpoint)
- CRUD for articles
