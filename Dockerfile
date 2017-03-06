FROM progaudi/dotnet:1.1-rc3-xenial

WORKDIR /app

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet --version \
    && ./scripts/build-netcore.sh \
    && ./scripts/test-netcore.sh