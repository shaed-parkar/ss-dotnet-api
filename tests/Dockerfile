FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy wait-for-it.sh into our image
ADD https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh /wait-for-it.sh
# Make it executable, in Linux
RUN chmod +x /wait-for-it.sh

COPY ["scripts/tests/run-domain-tests.sh", "/"]
COPY ["scripts/tests/run-dal-tests.sh", "/"]
COPY ["scripts/tests/run-api-tests.sh", "/"]
RUN chmod +x /run-domain-tests.sh
RUN chmod +x /run-dal-tests.sh
RUN chmod +x /run-api-tests.sh

# Copy project files to cache layer
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/SS.Api.Contracts/SS.Api.Contracts.csproj", "src/SS.Api.Contracts/"]
COPY ["src/SS.Common/SS.Common.csproj", "src/SS.Common/"]
COPY ["src/SS.DAL/SS.DAL.csproj", "src/SS.DAL/"]
COPY ["src/SS.Domain/SS.Domain.csproj", "src/SS.Domain/"]

COPY ["tests/Api.Tests/Api.Tests.csproj", "tests/Api.Tests/"]
COPY ["tests/SS.DAL.Tests/SS.DAL.Tests.csproj", "tests/SS.DAL.Tests/"]
COPY ["tests/SS.Domain.Tests/SS.Domain.Tests.csproj", "tests/SS.Domain.Tests/"]
COPY ["tests/SS.IntegrationTests.Common/SS.IntegrationTests.Common.csproj", "tests/SS.IntegrationTests.Common/"]

# Restore project files to cache layer
RUN dotnet restore "src/Api/Api.csproj"
RUN dotnet restore "src/SS.Api.Contracts/SS.Api.Contracts.csproj"
RUN dotnet restore "src/SS.Common/SS.Common.csproj"
RUN dotnet restore "src/SS.DAL/SS.DAL.csproj"
RUN dotnet restore "src/SS.Domain/SS.Domain.csproj"

RUN dotnet restore "tests/Api.Tests/Api.Tests.csproj"
RUN dotnet restore "tests/SS.DAL.Tests/SS.DAL.Tests.csproj"
RUN dotnet restore "tests/SS.Domain.Tests/SS.Domain.Tests.csproj"
RUN dotnet restore "tests/SS.IntegrationTests.Common/SS.IntegrationTests.Common.csproj"

# Add remaining files prone to change
COPY . .
RUN dotnet build src/SSApi.sln -c Release