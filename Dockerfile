# Set the base image as the .NET 6.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

# Copy everything and publish the release (publish implicitly restores and builds)
COPY . ./
RUN dotnet publish ./ssh-action/ssh-action.csproj -c Release -o out --no-self-contained

# Label the container
LABEL maintainer="Robert Swilley <robert@rswilley.com>"
LABEL repository="https://github.com/rswilley/ssh-action"
LABEL homepage="https://github.com/rswilley/ssh-action"

# Label as GitHub action
LABEL com.github.actions.name=".NET Core SSH Deploy"
LABEL com.github.actions.description=".NET Core SSH Deploy"
LABEL com.github.actions.icon="sliders"
LABEL com.github.actions.color="purple"

# Relayer the .NET SDK, anew with the build output
FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY --from=build-env /out .
ENTRYPOINT [ "dotnet", "/ssh-action.dll" ]