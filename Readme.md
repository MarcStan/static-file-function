# Serve static content via Azure function

This project demonstrates how to serve static content via an Azure function while also making use of http functions in the backend.

Example usecases could be an single page application (Angular, React, ..) that uses the function as its backend (skipping CORS issues because both are hosted on the same domain).

This is definitely usable for private projects, for enterprise solutions [setting up a CDN](https://marcstan.net/blog/2019/07/12/Static-websites-via-Azure-Storage-and-CDN/) to serve the frontend might be the better option.
