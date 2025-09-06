FROM macmenatcode/net-sdk-8-ef-core-8:latest as base
WORKDIR /db
COPY . .

FROM base as raise
CMD bash ./migrate_fill.sh