-- Runs once, automatically, the first time the mysql container starts with an empty data volume
-- (see docker-entrypoint-initdb.d in the official mysql image docs).
-- Creates the two empty databases the app's connection strings expect; EF Core migrations
-- (run by the `migrator` service) are what actually create the tables inside them.
CREATE DATABASE IF NOT EXISTS LexilearnDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE DATABASE IF NOT EXISTS IdentityDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
