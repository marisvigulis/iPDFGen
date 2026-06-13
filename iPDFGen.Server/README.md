## iPDFGen.Server

A source code of an [ipdfgenserver image](https://hub.docker.com/repository/docker/marisvigulis/ipdfgenserver)

## For contributors
To run a iPDFGen.Server locally, first, you need to install dependencies:
```
dotnet iPDFGen.Server.dll install-deps
```


## Some useful commands


### Build
```bash 
  docker build -t ipdfgen-server .
```

### Run
```bash
  docker run --rm -p 8080:8080 -e SHARED_SECRET=MAGIC_STRING_$2123499 ipdfgen-server
```
> All endpoints require an `X-Shared-Secret: my-secret` header matching `SHARED_SECRET`.
> On first start the container downloads Chromium before it begins listening on port `8080`.

Optional environment variables: `PDFGEN_PROVIDER` (`Playwright` or `Puppeteer`), `MAX_DEGREE_OF_PARALELLISM`, `DEFAULT_TIMEOUT` (seconds).


