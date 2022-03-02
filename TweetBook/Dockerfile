FROM mcr.microsoft.com/dotnet/sdk:6.0.200 as build
#FROM microsoft/dotnet:5.0-sdk as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY TweetBook.csproj /build/

RUN dotnet restore ./build/TweetBook.csproj

COPY . ./build/ 
WORKDIR /build/
RUN dotnet publish ./TweetBook.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:6.0
#FROM microsoft/dotnet:5.0-aspnetcore-runtime
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "TweetBook.dll"]