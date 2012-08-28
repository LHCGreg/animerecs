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

  start_year smallint NULL,
  start_month smallint NULL,
  start_day smallint NULL,

  end_year smallint NULL,
  end_month smallint NULL,
  end_day smallint NULL,

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
  mal_list_entry_status_id smallint NOT NULL PRIMARY KEY,
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
  rating smallint NULL,
  mal_list_entry_status_id smallint NOT NULL, -- Not a foreign key because MAL could add more statuses
  num_episodes_watched smallint NOT NULL,
  started_watching_year smallint NULL,
  started_watching_month smallint NULL,
  started_watching_day smallint NULL,
  finished_watching_year smallint NULL,
  finished_watching_month smallint NULL,
  finished_watching_day smallint NULL,
  last_mal_update timestamp with time zone NOT NULL
);

CREATE INDEX index_mal_list_entry__mal_anime_id
  ON mal_list_entry (mal_anime_id);

CREATE UNIQUE INDEX index_mal_list_entry__mal_user_id__mal_anime_id
  ON mal_list_entry (mal_user_id, mal_anime_id);

CREATE TABLE mal_list_entry_tag
(
  mal_list_entry_tag_id serial NOT NULL PRIMARY KEY,
  mal_user_id int NOT NULL REFERENCES mal_user (mal_user_id) ON DELETE CASCADE,
  mal_anime_id int NOT NULL REFERENCES mal_anime (mal_anime_id),
  tag text NOT NULL
);

CREATE INDEX index_mal_list_entry_tag__mal_user_id__mal_anime_id
  ON mal_list_entry_tag (mal_user_id, mal_anime_id);

CREATE TABLE streaming_service
(
	streaming_service_id int NOT NULL PRIMARY KEY,
	name text NOT NULL,
	url text NOT NULL
);

INSERT INTO streaming_service
(streaming_service_id, name, url)
VALUES
(1, 'Crunchyroll', 'http://www.crunchyroll.com/'),
(2, 'Funimation', 'http://www.funimation.com/videos');

CREATE TABLE streaming_service_anime_map
(
	streaming_service_anime_map_id serial NOT NULL PRIMARY KEY,
	mal_anime_id int NOT NULL, -- No foreign key in order to allow anime that is not in the database but is on MAL
	streaming_service_id int NOT NULL REFERENCES streaming_service (streaming_service_id),
	streaming_url text NOT NULL,
	requires_subscription boolean NOT NULL
);

CREATE UNIQUE INDEX index_anime_service_url
  ON streaming_service_anime_map
  (mal_anime_id, streaming_service_id, streaming_url);
  
CREATE INDEX index_service_subscription
  ON streaming_service_anime_map
  (streaming_service_id, requires_subscription);
  
CREATE INDEX index_subscription
  ON streaming_service_anime_map
  (requires_subscription);

COMMIT TRANSACTION;