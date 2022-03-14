### my-web

My personal webapp for learning purpose.

The stack used for this webapp:

- .net 6 (backend)
- react (frontend)
  - Page template -> [Chakra UI template](https://chakra-templates.dev/)
- PostgreSQL (database)

This webapp has these functions:

- Basic authentication with jwt token and refresh token (by integrating Identity Core and [OpenIddict](https://github.com/openiddict/openiddict-core))
- Forgot and reset password function.

Currently hosted at [Digital Ocean droplet](http://164.92.65.152/)

- Use Docker to run dockerized backend and database.
- Use Nginx to host frontend and also act as reverse proxy (forward api request to backend).
