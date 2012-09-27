BEGIN TRANSACTION;

CREATE TABLE mal_anime_prerequisite
(
  mal_anime_prerequisite_id serial NOT NULL PRIMARY KEY,

  -- These are not foreign keys because it is possible for an anime on MAL to not be in the mal_anime table yet
  mal_anime_id int NOT NULL,
  prerequisite_mal_anime_id int NOT NULL
);

CREATE UNIQUE INDEX index_anime__prerequisite
  ON mal_anime_prerequisite (mal_anime_id, prerequisite_mal_anime_id);

COMMIT TRANSACTION;