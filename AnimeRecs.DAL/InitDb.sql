-- For PostgreSQL only

BEGIN TRANSACTION;

CREATE TABLE mal_user
(
  mal_user_id int NOT NULL PRIMARY KEY,
  mal_name text NOT NULL,
  time_added timestamp with time zone NOT NULL
);

CREATE INDEX index_mal_user_time_added
  ON mal_user (time_added);

CREATE INDEX index_mal_user_mal_name
  ON mal_user (mal_name);

CREATE TABLE mal_anime_type
(
  mal_anime_type_id int NOT NULL PRIMARY KEY,
  type_name text NOT NULL
);

INSERT INTO mal_anime_type
(mal_anime_type_id, type_name)
VALUES
(1, 'TV'),
(2, 'OVA'),
(3, 'Movie'),
(4, 'Special'),
(5, 'ONA');

CREATE TABLE mal_anime_status
(
  mal_anime_status_id int NOT NULL PRIMARY KEY,
  status_name text NOT NULL
);

INSERT INTO mal_anime_status
(mal_anime_status_id, status_name)
VALUES
(1, 'Airing'),
(2, 'Finished Airing'),
(3, 'Not Yet Aired');

CREATE TABLE mal_anime
(
  mal_anime_id int NOT NULL PRIMARY KEY,
  title text NOT NULL,
  mal_anime_type_id int NOT NULL, -- not a foreign key because MAL could add new types
  num_episodes int NOT NULL,
  mal_anime_status_id int NOT NULL, -- not a foreign key because MAL could add new status types,

  start_year int NULL,
  start_month int NULL,
  start_day int NULL,

  end_year int NULL,
  end_month int NULL,
  end_day int NULL,

  image_url text NOT NULL,
  last_updated timestamp with time zone NOT NULL
);

CREATE TABLE mal_anime_synonym
(
  mal_anime_synonym_id serial NOT NULL PRIMARY KEY,
  mal_anime_id int NOT NULL REFERENCES mal_anime ON DELETE CASCADE,
  synonym text NOT NULL
);

CREATE INDEX index_mal_anime_synonym__mal_anime_id
  ON mal_anime_synonym (mal_anime_id);

CREATE TABLE mal_list_entry_status
(
  mal_list_entry_status_id int NOT NULL PRIMARY KEY,
  status_name text
);

INSERT INTO mal_list_entry_status
(mal_list_entry_status_id, status_name)
VALUES
(1, 'Watching'),
(2, 'Completed'),
(3, 'On-Hold'),
(4, 'Dropped'),
(6, 'Plan to Watch'); -- No, this is not a typo, it does skip from 4 to 6

CREATE TABLE mal_list_entry
(
  mal_list_entry_id serial NOT NULL PRIMARY KEY,
  mal_user_id int NOT NULL REFERENCES mal_user (mal_user_id) ON DELETE CASCADE,
  mal_anime_id int NOT NULL REFERENCES mal_anime (mal_anime_id),
  rating numeric NULL,
  mal_list_entry_status_id int NOT NULL, -- Not a foreign key because MAL could add more statuses
  num_episodes_watched int NOT NULL,
  started_watching_year int NULL,
  started_watching_month int NULL,
  started_watching_day int NULL,
  finished_watching_year int NULL,
  finished_watching_month int NULL,
  finished_watching_day int NULL,
  last_mal_update timestamp with time zone NOT NULL
);

CREATE INDEX index_mal_list_entry__mal_anime_id
  ON mal_list_entry (mal_anime_id);

CREATE UNIQUE INDEX index_mal_list_entry__mal_user_id__mal_anime_id
  ON mal_list_entry (mal_user_id, mal_anime_id);

CREATE INDEX index_mal_list_entry__rating
  ON mal_list_entry (rating);

CREATE INDEX index_mal_list_entry__mal_list_entry_status_id
  ON mal_list_entry (mal_list_entry_status_id);

CREATE INDEX index_mal_list_entry__num_episodes_watched
  ON mal_list_entry (num_episodes_watched);

CREATE TABLE mal_list_entry_tag
(
  mal_list_entry_tag_id serial NOT NULL PRIMARY KEY,
  mal_user_id int NOT NULL REFERENCES mal_user (mal_user_id) ON DELETE CASCADE,
  mal_anime_id int NOT NULL REFERENCES mal_anime (mal_anime_id),
  tag text NOT NULL
);

CREATE INDEX index_mal_list_entry_tag__mal_user_id__mal_anime_id
  ON mal_list_entry_tag (mal_user_id, mal_anime_id);

-- Keep a row count table because a SELECT count(*) FROM some_table does a table scan in Postgres.
-- Don't bother for tables like mal_anime_type because they're tiny.
CREATE TABLE row_count
(
  table_id int NOT NULL PRIMARY KEY,
  table_name text NOT NULL,
  num_rows bigint NOT NULL
);

INSERT INTO row_count
(table_id, table_name, num_rows)
VALUES
(1, 'mal_user', 0),
(2, 'mal_anime', 0);

CREATE FUNCTION update_row_count()
  RETURNS trigger AS
$BODY$
BEGIN
    IF TG_OP = 'INSERT' THEN
        UPDATE row_count
            SET num_rows = num_rows + 1
            WHERE table_name = TG_RELNAME;
    ELSIF TG_OP = 'DELETE' THEN
        UPDATE row_count
            SET num_rows = num_rows - 1
            WHERE table_name = TG_RELNAME;
    END IF;
    RETURN NULL;
END
$BODY$
  LANGUAGE plpgsql VOLATILE;

CREATE TRIGGER trigger_update_row_count
  AFTER INSERT OR DELETE
  ON mal_user
  FOR EACH ROW
  EXECUTE PROCEDURE update_row_count();

CREATE TRIGGER trigger_update_row_count
  AFTER INSERT OR DELETE
  ON mal_anime
  FOR EACH ROW
  EXECUTE PROCEDURE update_row_count();

-- Not used by any code, just useful to check.
CREATE VIEW mal_average_ratings
(mal_anime_id, title, average, num_ratings)
AS
SELECT * FROM
(SELECT anime.mal_anime_id, anime.title, avg(entry.rating) AS average, count(*) AS num_ratings
FROM mal_list_entry AS entry
JOIN mal_anime AS anime ON anime.mal_anime_id = entry.mal_anime_id
WHERE entry.rating IS NOT NULL
AND (entry.mal_list_entry_status_id = 2 OR entry.num_episodes_watched > 26)
GROUP BY anime.mal_anime_id
ORDER BY average DESC) AS averageStuff
WHERE average IS NOT NULL AND num_ratings >= ((SELECT num_rows FROM row_count WHERE table_id = 1) / 50); -- >= 2% of users have seen it

COMMIT TRANSACTION;