BEGIN TRANSACTION;

INSERT INTO streaming_service
(streaming_service_id, name, url)
VALUES
(4, 'Hulu', 'http://www.hulu.com');

COMMIT TRANSACTION;