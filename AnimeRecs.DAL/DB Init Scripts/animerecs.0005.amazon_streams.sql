BEGIN TRANSACTION;

INSERT INTO streaming_service
(streaming_service_id, name, url)
VALUES
(9, 'Amazon Prime', 'https://www.amazon.com/s/ref=sr_rot_p_n_ways_to_watch_1?rh=n:2858778011,p_n_theme_browse-bin:2650364011,p_n_ways_to_watch:12007865011&ie=UTF8'),
(10, 'Amazon Anime Strike', 'https://www.amazon.com/s/ref=sr_nr_p_n_subscription_id_1?rh=n:2858778011,p_n_ways_to_watch:12007866011,p_n_subscription_id:16182082011&ie=UTF8');

COMMIT TRANSACTION;