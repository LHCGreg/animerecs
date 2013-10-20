using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.UpdateStreams.Tests
{
    public partial class FunimationStreamInfoSourceTests
    {
        public static string TestHtml = @"


<!DOCTYPE html>
<html lang=""en-us"">
<head>
<meta charset=""utf-8""/>
<meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1"">
<script type=""text/javascript"">
//<![CDATA[
try{if (!window.CloudFlare) { var CloudFlare=[{verbose:0,p:1382128629,byc:0,owlid:""cf"",bag2:1,mirage2:0,oracle:0,paths:{cloudflare:""/cdn-cgi/nexp/abv=1309062649/""},atok:""e0abdfdd0d740e488cfb38c89144de06"",petok:""0e98875f0f43feef28d198dce6a9a256-1382237409-1800"",zone:""funimation.com"",rocket:""a"",apps:{}}];document.write('<script type=""text/javascript"" src=""//ajax.cloudflare.com/cdn-cgi/nexp/abv=3224043168/cloudflare.min.js""><'+'\/script>')}}catch(e){};
//]]>
</script>
<script type=""text/rocketscript"">var NREUMQ=NREUMQ||[];NREUMQ.push([""mark"",""firstbyte"",new Date().getTime()]);</script><link rel=""shortcut icon"" href=""http://www.funimation.com/assets/img/funimation2_favicon_0.ico"" type=""image/x-icon"">
 
<meta property=""og:url"" content=""http://www.funimation.com/videos""/>
<meta property=""og:site_name"" content=""Funimation"">
<script data-rocketsrc=""http://www.funimation.com/assets/js/head.load.min.js"" type=""text/rocketscript""></script>
<script type='text/rocketscript'>
        var googletag = googletag || {};
        googletag.cmd = googletag.cmd || [];
        (function() {
            var gads = document.createElement('script');
            gads.async = true;
            gads.type = 'text/javascript';
            var useSSL = 'https:' == document.location.protocol;
            gads.src = (useSSL ? 'https:' : 'http:') +
                '//www.googletagservices.com/tag/js/gpt.js';
            var node = document.getElementsByTagName('script')[0];
            node.parentNode.insertBefore(gads, node);
        })();
    </script>
<script type='text/rocketscript'>

            googletag.cmd.push(function() {
                googletag.defineSlot('/82442012/funimation_big_box', [300, 250], 'div-gpt-ad-1374328695293-0').addService(googletag.pubads());
                googletag.defineSlot('/82442012/funimation_big_box_2', [300, 250], 'div-gpt-ad-1377114716006-1').addService(googletag.pubads());
                googletag.pubads().enableSingleRequest();
                googletag.enableServices();
            });

    </script>
<script type=""text/rocketscript"">
	
	  var _gaq = _gaq || [];
	  _gaq.push(['_setAccount', 'UA-39725952-1']);
	  _gaq.push(['_trackPageview']);
	
	  (function() {
	    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
	    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
	    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
	  })();
	
	</script>
<link type=""text/css"" rel=""stylesheet"" href=""http://www.funimation.com/assets/css/styles.css""/><link type=""text/css"" rel=""stylesheet"" href=""http://www.funimation.com/assets/css/widget_show_featured_product.css""/><meta property=""og:image"" content=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7555862/2_thumbnail/EUR0001.jpg""/><meta property=""og:image"" content=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7555922/2_thumbnail/FMB0001.jpg""/><meta property=""og:image"" content=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7555978/2_thumbnail/GTC0001.jpg""/><title>Watch Anime at FUNimation - Watch Streaming Anime Episodes Free</title>
<meta name=""description"" content=""Watch One Piece, DBZ, Hetalia anime episodes online free at FUNimation English dubbed and subtitled. Watch anime movies and trailers in HD."">
<meta property=""og:title"" content=""Watch Anime at FUNimation - Watch Streaming Anime Episodes Free"">
<meta property=""og:description"" content=""Watch One Piece, DBZ, Hetalia anime episodes online free at FUNimation English dubbed and subtitled. Watch anime movies and trailers in HD."">
<meta property=""og:type"" content=""article"">
</head>
<body id=""video-page"">
 
<div id=""unlogged-opened"">
<header>
<section class=""center-layout"">
<article class=""left"">
<span class=""white left"">Log in to watch free streaming video, <br>
post in the forums, peep behind the <br>
scenes exclusives, and so much more!
</span>
</article>
<form action=""http://www.funimation.com/login"" method=""post"" accept-charset=""utf-8"" class=""right loginForm""> <input type=""hidden"" name=""return"" value=""http%3A%2F%2Fwww.funimation.com%2Fvideos""/>
<input type=""hidden"" name=""logged"" value=""1""/>
<div style=""float:left;"">
<div class=""login_error"">
<label for=""email_field"" class=""white"">Email</label>
<input type=""text"" name=""email_field"" tabindex=""1""/>
</div>
<div class=""login_error"">
<label for=""password_field"" class=""white"">Password</label>
<input type=""password"" name=""password_field"" tabindex=""2""/>
</div>
<div class=""login_error_show"">
<p>
<a href=""http://www.funimation.com/loadModal/index/forgot_password_modal"" data-fancybox-width='630' class=""white fs12 fancybox fancybox.iframe"">Forgot password?</a>
</p>
</div>
</div>
<span class=""left"">
<button class=""btn-red size3 left buttonLogin"" type="""" tabindex=""3"">Log In</button>
</span>
</form> </section>
</header>
</div>
 
<script type=""text/rocketscript"">
        head.ready(function(e){
            $('.buttonLogin').on('click', function(e){
                e.preventDefault();
                var error = false;
                if ($('.loginForm input[name=""email_field""]').val() == ''){
                    alertify.error('Please enter your email to login!');
                    error = true;
                }
                if ($('.loginForm input[name=""password_field""]').val() == ''){
                    alertify.error('Please enter your password to login!');
                    error = true;
                }
                if (error == false){
                    $('.loginForm').submit();
                }

            })

        })
    </script>
<section class=""top-main-section"">
 
<div id=""video_top_slider""></div>
<ul class=""coverflow-control-nav center"">
</ul>
<ul class=""coverflow-direction-nav"">
<li class=""left"">
<a href=""javascript:;"" onClick=""coverflow().prev();"" class=""previous"">Previous</a>
</li>
<li class=""right"">
<a href=""javascript:;"" onClick=""coverflow().next();"" class=""next"">Next</a>
</li>
</ul>
 </section>
<nav id=""main-nav-module"">
<section class=""header"">
<a class=""logo left"" href=""http://www.funimation.com/""></a>
<span class=""main-tagline left bold"">YOU SHOULD BE WATCHING</span>
<article class=""social-lnks left"">
<a href=""http://www.facebook.com/funimation"" target=""_blank"" class=""left""><span class=""social foundicon-facebook""></span></a>
<a href=""http://www.twitter.com/funimation"" target=""_blank"" class=""left""><span class=""social foundicon-twitter""></span></a>
<a href=""https://www.youtube.com/funimation"" target=""_blank"" class=""left""><span class=""social foundicon-youtube""></span></a>
</article>
<a href=""#"" id=""login-btn"" class=""login-btn t-center bold white"">Log In</a>
<a href=""http://www.funimation.com/join_now"" class=""btn-red size3 right"">Join Now</a>
</section> <section class=""main-navigation-1"">
<ul class=""main-nav bold left"">
<li>
<a href=""http://www.funimation.com/shows"" class=""white "">Shows</a>
</li>
<li>
<a href=""http://www.funimation.com/videos"" class=""white current"">Videos</a>
</li>
<li>
<a href=""http://www.funimation.com/schedule"" class=""white "">Schedule</a>
</li>
<li>
<a href=""http://www.funimation.com/blog"" class=""white "">Blog</a>
</li>
<li>
<a href=""http://shop.funimation.com/Shop/ShopMain.html"" class=""white "">Shop</a>
</li>
</ul>
<a href=""http://shop.funimation.com/Shop/cart.ssp"" class=""icon-shopping-cart right""><span class=""fs12 condensed bold""></span></a>
<form class=""search-box right"" action=""http://www.funimation.com/search"" method=""get"">
<input class=""field left"" type=""text"" name=""q"" value="""" placeholder=""Search""/>
<input class=""button right"" type=""submit"" value=""Search""/>
</form>
<ul class=""basic-nav bold right"">
<li>
<a href=""http://www.funimation.com/apps/anime"" class="""">Apps</a>
</li>
<li>
<a href=""http://www.funimation.com/forum/forum.php""> Forum</a>
</li>
<li>
<a href=""http://www.funimation.com/games/anime"" class="""">Games</a>
</li>
<li>
<a href=""http://www.funimation.com/tv/anime"" class="""">TV</a>
</li>
<li>
<a class=""subscribe-lnk "" href=""http://www.funimation.com/subscribe/anime"">Subscribe</a>
</li>
</ul>
</section>
<div class=""main-navigation-2"">
<section class=""navigation_videos clearfix"">
<span id=""arrow""></span>
<ul class=""main-nav left fs14 bold"">
<li>
<a href=""http://www.funimation.com/videos/episodes"" class=""white "">Episodes</a>
</li>
<li>
<a href=""http://www.funimation.com/videos/movies"" class=""white "">Movies</a>
</li>
<li>
<a href=""http://www.funimation.com/videos/trailers"" class=""white "">Trailers</a>
</li>
<li>
<a href=""http://www.funimation.com/videos/clips"" class=""white "">Clips</a>
</li>
<li>
<a href=""http://www.funimation.com/videos/extras"" class=""white "">Extras</a>
</li>
<li>
<a href=""http://www.funimation.com/videos/playlists"" class=""white "">Playlists</a>
</li>
<li>
<a href=""http://www.funimation.com/videos/simulcasts"" class=""white "">Simulcasts</a>
</li>
</ul>
<ul class=""basic-nav right"">
<li>
<p>Spend $50, USA FREE SHIPPING</p>
</li>
</ul>
</section>
</div>
</nav>
<section id=""main-wrapper"" class=""center-layout"">
<section id=""main-content"">
 
<article class=""column_2 shadow-line normal-slider double-row flexslider"">
<div class=""basic-heading"">
<h2>EPISODES</h2>
<a href=""http://www.funimation.com/videos/episodes"" class=""btn size1 viewAll"" data-showid="""" data-section=""episodes"">View All</a>
<img src=""http://www.funimation.com/assets/img/loading.gif"" class=""loading"" style=""display: none"">
</div>
<div>
<ul class=""items-row slides clearfix"">
<li><ul> <li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/eureka-seven-astral-ocean/deep-blue"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/eureka-seven-astral-ocean/videos/official/deep-blue/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7555862/2_thumbnail/EUR0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/eureka-seven-astral-ocean/anime"" class=""item-title"">Eureka Seven AO</a>
<a href=""http://www.funimation.com/shows/eureka-seven-astral-ocean/videos/official/deep-blue/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
deep blue </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/eureka-seven-astral-ocean/anime"" class=""item-title"">Eureka Seven AO</a>
<a href=""http://www.funimation.com/shows/eureka-seven-astral-ocean/videos/official/deep-blue/anime"" class=""item-title"">Episode 1 - deep blue</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 24:37</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Ao is one day away from starting middle school when his world begins to change. Scub Coral activity leads to Scub Bursts and mysterious G-Monsters. And when he picks up an odd bracelet, he finds himself drawn even deeper into these events.</p>
<p>
<a href=""http://www.funimation.com/shows/eureka-seven-astral-ocean/videos/official/deep-blue/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/eureka-seven-astral-ocean/videos/official/deep-blue/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/fullmetal-alchemist-brotherhood/fullmetal-alchemist"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/fullmetal-alchemist-brotherhood/videos/official/fullmetal-alchemist/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7555922/2_thumbnail/FMB0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/fullmetal-alchemist-brotherhood/anime"" class=""item-title"">Fullmetal Alchemist: Brotherhood</a>
<a href=""http://www.funimation.com/shows/fullmetal-alchemist-brotherhood/videos/official/fullmetal-alchemist/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
Fullmetal Alchemist </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/fullmetal-alchemist-brotherhood/anime"" class=""item-title"">Fullmetal Alchemist: Brotherhood</a>
<a href=""http://www.funimation.com/shows/fullmetal-alchemist-brotherhood/videos/official/fullmetal-alchemist/anime"" class=""item-title"">Episode 1 - Fullmetal Alchemist</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 24:40</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>The Elric brothers adjust to military life and take part in a manhunt for the dangerous Isaac the Freezer, a former State Alchemist bent on bringing Fuhrer Bradley down.</p>
<p>
<a href=""http://www.funimation.com/shows/fullmetal-alchemist-brotherhood/videos/official/fullmetal-alchemist/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/fullmetal-alchemist-brotherhood/videos/official/fullmetal-alchemist/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/guilty-crown/genesis-outbreak"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/official/genesis-outbreak/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7555978/2_thumbnail/GTC0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/guilty-crown/anime"" class=""item-title"">Guilty Crown</a>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/official/genesis-outbreak/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
genesis (Outbreak) </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/guilty-crown/anime"" class=""item-title"">Guilty Crown</a>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/official/genesis-outbreak/anime"" class=""item-title"">Episode 1 - genesis (Outbreak)</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 23:01</p>
<div class=""rank rank-4-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Shu enters his hideout for a quiet lunch only to find Inori, his #1 musical crush, bleeding on the floor. After he fails to stop her pursuers from hurting her more, his one shot at self-respect is to deliver a strange vial she stole to her terrorist pals.</p>
<p>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/official/genesis-outbreak/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/guilty-crown/videos/official/genesis-outbreak/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/haganai/we-cant-make-any-friends"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/haganai/videos/official/we-cant-make-any-friends/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7556002/2_thumbnail/HAA0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/haganai/anime"" class=""item-title"">Haganai</a>
<a href=""http://www.funimation.com/shows/haganai/videos/official/we-cant-make-any-friends/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
We Can't Make Any Friends </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/haganai/anime"" class=""item-title"">Haganai</a>
<a href=""http://www.funimation.com/shows/haganai/videos/official/we-cant-make-any-friends/anime"" class=""item-title"">Episode 1 - We Can't Make Any Friends</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 24:26</p>
<div class=""rank rank-4-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Yozora and Kodoka are loners at school. In order to make friends with other misfits, they start the Neighbors Club. Soon, a third member joins the club: Sena, a popular girl who annoys Yozora to no end.</p>
<p>
<a href=""http://www.funimation.com/shows/haganai/videos/official/we-cant-make-any-friends/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/haganai/videos/official/we-cant-make-any-friends/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/high-school-dxd/i-got-a-girlfriend"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/high-school-dxd/videos/official/i-got-a-girlfriend/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7556046/2_thumbnail/DXD0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/high-school-dxd/anime"" class=""item-title"">High School DxD</a>
<a href=""http://www.funimation.com/shows/high-school-dxd/videos/official/i-got-a-girlfriend/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
I Got a Girlfriend! </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/high-school-dxd/anime"" class=""item-title"">High School DxD</a>
<a href=""http://www.funimation.com/shows/high-school-dxd/videos/official/i-got-a-girlfriend/anime"" class=""item-title"">Episode 1 - I Got a Girlfriend!</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 23:55</p>
<div class=""rank rank-4-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Issei Hyodo's first date with his new girlfriend ends poorly when she turns into a devil and stabs him in the stomach. Luckily, he's saved by Rias, the buxom president of his school's Occult Research Club. And then, things really start to get weird…</p>
<p>
<a href=""http://www.funimation.com/shows/high-school-dxd/videos/official/i-got-a-girlfriend/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/high-school-dxd/videos/official/i-got-a-girlfriend/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/lupin-the-third-fujiko-mine/master-thief-vs-lady-looter"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/official/master-thief-vs-lady-looter/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7556182/2_thumbnail/LPN0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/anime"" class=""item-title"">Lupin the Third - The Woman Called Fujiko Mine</a>
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/official/master-thief-vs-lady-looter/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
Master Thief vs. Lady Looter </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/anime"" class=""item-title"">Lupin the Third - The Woman Called Fujiko Mine</a>
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/official/master-thief-vs-lady-looter/anime"" class=""item-title"">Episode 1 - Master Thief vs. Lady Looter</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 23:11</p>
<div class=""rank rank-6-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Fujiko Mine, the devastatingly sexy lady thief who'll stop at nothing to make her steal, has her sights set on a religious cult's secret treasure when a new man waltzes into her life: Lupin the Third. Has our luscious looter finally met her match?</p>
<p>
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/official/master-thief-vs-lady-looter/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/official/master-thief-vs-lady-looter/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<span class=""e"">e</span>
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/michiko-hatchin/farewell-cruel-paradise"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/michiko-hatchin/videos/official/farewell-cruel-paradise/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7556202/2_thumbnail/MNH0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/michiko-hatchin/anime"" class=""item-title"">Michiko & Hatchin</a>
<a href=""http://www.funimation.com/shows/michiko-hatchin/videos/official/farewell-cruel-paradise/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
Farewell, Cruel Paradise! </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/michiko-hatchin/anime"" class=""item-title"">Michiko & Hatchin</a>
<a href=""http://www.funimation.com/shows/michiko-hatchin/videos/official/farewell-cruel-paradise/anime"" class=""item-title"">Episode 1 - Farewell, Cruel Paradise!</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 22:42</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Michiko Malandro breaks out of a high-security prison to see her long-lost love, Hiroshi. Elsewhere, Hiroshi's daughter, Hana Morenos, lives the life of Cinderella among her adoptive family, waiting for someone to come and take her away from them.</p>
<p>
<a href=""http://www.funimation.com/shows/michiko-hatchin/videos/official/farewell-cruel-paradise/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/michiko-hatchin/videos/official/farewell-cruel-paradise/anime?watch=dub"" class=""elite"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/ouran-high-school-host-club/starting-today-you-are-a-host"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/official/starting-today-you-are-a-host/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7556294/2_thumbnail/OUR0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/anime"" class=""item-title"">Ouran High School Host Club</a>
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/official/starting-today-you-are-a-host/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
Starting Today, You Are a Host! </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/anime"" class=""item-title"">Ouran High School Host Club</a>
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/official/starting-today-you-are-a-host/anime"" class=""item-title"">Episode 1 - Starting Today, You Are a Host!</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 23:27</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>After Haruhi literally stumbles in on the Ouran Host Club and breaks an expensive vase, she discovers there’s only one way she’ll ever be able to pay for the damage - working as a Host!</p>
<p>
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/official/starting-today-you-are-a-host/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/official/starting-today-you-are-a-host/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
</ul> </li>
<li><ul> <li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/the-future-diary/sign-up"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/the-future-diary/videos/official/sign-up/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7556582/2_thumbnail/FDY0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/the-future-diary/anime"" class=""item-title"">The Future Diary</a>
<a href=""http://www.funimation.com/shows/the-future-diary/videos/official/sign-up/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
Sign Up </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/the-future-diary/anime"" class=""item-title"">The Future Diary</a>
<a href=""http://www.funimation.com/shows/the-future-diary/videos/official/sign-up/anime"" class=""item-title"">Episode 1 - Sign Up</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 24:09</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Yukiteru spends his days writing into his cell phone ""diary,"" a listing of the things that happen around him. When it suddenly starts telling him everything that is going to happen before it does, he's thrilled. Until it tells him he's going to die!</p>
<p>
<a href=""http://www.funimation.com/shows/the-future-diary/videos/official/sign-up/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/the-future-diary/videos/official/sign-up/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/we-without-wings/for-example-that-kind-of-fairytale"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/we-without-wings/videos/official/for-example-that-kind-of-fairytale/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7556674/2_thumbnail/WWW0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/we-without-wings/anime"" class=""item-title"">We Without Wings</a>
<a href=""http://www.funimation.com/shows/we-without-wings/videos/official/for-example-that-kind-of-fairytale/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
For Example, That Kind of Fairytale </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/we-without-wings/anime"" class=""item-title"">We Without Wings</a>
<a href=""http://www.funimation.com/shows/we-without-wings/videos/official/for-example-that-kind-of-fairytale/anime"" class=""item-title"">Episode 1 - For Example, That Kind of Fairytale</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 24:41</p>
<div class=""rank rank-4-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>DJ Condor is your guide as the everyday lives of several young city-dwellers come into focus on the streets of Yanagihara, a bustling metropolis where there’s no shortage of stories.</p>
<p>
<a href=""http://www.funimation.com/shows/we-without-wings/videos/official/for-example-that-kind-of-fairytale/anime?watch=sub"">Watch Sub</a>
|
<a href=""http://www.funimation.com/shows/we-without-wings/videos/official/for-example-that-kind-of-fairytale/anime?watch=dub"">Watch Dub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/the-devil-is-a-part-timer/the-devil-arrives-in-sasazuka"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/the-devil-is-a-part-timer/videos/official/the-devil-arrives-in-sasazuka/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/recap_thumbnails/7556737/2_thumbnail/DPT0001.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/the-devil-is-a-part-timer/anime"" class=""item-title"">The Devil is a Part-Timer!</a>
<a href=""http://www.funimation.com/shows/the-devil-is-a-part-timer/videos/official/the-devil-arrives-in-sasazuka/anime"" class=""brief with-middle-line"">
<span>Episode 1</span>
The Devil Arrives in Sasazuka </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/the-devil-is-a-part-timer/anime"" class=""item-title"">The Devil is a Part-Timer!</a>
<a href=""http://www.funimation.com/shows/the-devil-is-a-part-timer/videos/official/the-devil-arrives-in-sasazuka/anime"" class=""item-title"">Episode 1 - The Devil Arrives in Sasazuka</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 23:45</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>When the lord of demons and his horde waged war against the humans in their realm, a hero vanquished their army. Cornered, the Devil fled through an interdimensional portal to Earth. He vows to one day reign again, but until then... he needs a job!
</p>
<p>
<a href=""http://www.funimation.com/shows/the-devil-is-a-part-timer/videos/official/the-devil-arrives-in-sasazuka/anime?watch=sub"">Watch Sub</a>
</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
</ul>
</div>
</article>
 
 
<article class=""column_2 shadow-line normal-slider flexslider"">
<div class=""basic-heading"">
<h2>CLIPS</h2>
<a href=""http://www.funimation.com/videos/clips"" class=""btn size1 viewAll"" data-showid="""" data-section=""clips"">View All</a>
<img src=""http://www.funimation.com/assets/img/loading.gif"" class=""loading"" style=""display: none"">
</div>
<ul class=""items-row slides clearfix"">
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/lastexile-fam-the-silver-wing/skydiving"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/videos/promotional/skydiving/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556154/2_thumbnail/6789abcdefghijklmnopqrstuvwxyzABCDEFGHI_k81399y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Skydiving""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/anime"" class=""item-title"">LASTEXILE -Fam, the Silver Wing</a>
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/videos/promotional/skydiving/anime"" class=""brief with-middle-line"">
<span>Clip</span>
Skydiving </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/anime"" class=""item-title"">LASTEXILE -Fam, the Silver Wing</a>
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/videos/promotional/skydiving/anime"" class=""item-title"">Clip - Skydiving</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 01:30</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>The Next Chapter in the Last Exile Saga.
Years ago, humanity abandoned the ruined Earth. Generations later, with the planet again capable of sustaining life, mankind returned. In the skies above the reborn world, rebell...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/wolf-children/raising-wolves-english"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/wolf-children/videos/promotional/raising-wolves-english/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556686/2_thumbnail/cdefghijklmnopqr_k48882y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Raising Wolves (English)""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/wolf-children/anime"" class=""item-title"">Wolf Children</a>
<a href=""http://www.funimation.com/shows/wolf-children/videos/promotional/raising-wolves-english/anime"" class=""brief with-middle-line"">
<span>Clip</span>
Raising Wolves (English) </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/wolf-children/anime"" class=""item-title"">Wolf Children</a>
<a href=""http://www.funimation.com/shows/wolf-children/videos/promotional/raising-wolves-english/anime"" class=""item-title"">Clip - Raising Wolves (English)</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-PG | 02:21</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
Experience the latest masterpiece from the internationally-acclaimed, award-winning director of Summer Wars. Now showing in select film festivals and conventions. For locations and show dates, consult the official w...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/high-school-dxd/the-house-of-gremory"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/high-school-dxd/videos/promotional/the-house-of-gremory/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556046/2_thumbnail/tuvwxyz_k25317y.png"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""The House of Gremory""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/high-school-dxd/anime"" class=""item-title"">High School DxD</a>
<a href=""http://www.funimation.com/shows/high-school-dxd/videos/promotional/the-house-of-gremory/anime"" class=""brief with-middle-line"">
<span>Clip</span>
The House of Gremory </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/high-school-dxd/anime"" class=""item-title"">High School DxD</a>
<a href=""http://www.funimation.com/shows/high-school-dxd/videos/promotional/the-house-of-gremory/anime"" class=""item-title"">Clip - The House of Gremory</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 01:43</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
A war between heaven and hell is raging on Earth &ndash; and a hot mess of hormonal fury is raging in Issei&rsquo;s pants. The guy is dying to get some action. Which is funny, since his first date ever turns into a...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/fairy-tail/character-spot-erza"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/character-spot-erza/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555878/2_thumbnail/_k73696y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Character Spot - Erza""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/fairy-tail/anime"" class=""item-title"">Fairy Tail</a>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/character-spot-erza/anime"" class=""brief with-middle-line"">
<span>Clip</span>
Character Spot - Erza </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/fairy-tail/anime"" class=""item-title"">Fairy Tail</a>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/character-spot-erza/anime"" class=""item-title"">Clip - Character Spot - Erza</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 00:15</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>This isn't your kid sister's dress-up. Erza's ""requip"" magic makes her a force to be reckoned with-- and then some</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/black-lagoon/roberta-s-blood-trail-clip-or-else"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/black-lagoon/videos/promotional/roberta-s-blood-trail-clip-or-else/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555674/2_thumbnail/klmnopqrstuvwxyzABCDEFGHIJKLMN_k58868y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Roberta's Blood Trail Clip: Or Else...""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/black-lagoon/anime"" class=""item-title"">Black Lagoon</a>
<a href=""http://www.funimation.com/shows/black-lagoon/videos/promotional/roberta-s-blood-trail-clip-or-else/anime"" class=""brief with-middle-line"">
<span>Clip</span>
Roberta's Blood Trail Clip: Or Else..... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/black-lagoon/anime"" class=""item-title"">Black Lagoon</a>
<a href=""http://www.funimation.com/shows/black-lagoon/videos/promotional/roberta-s-blood-trail-clip-or-else/anime"" class=""item-title"">Clip - Roberta's Blood Trail Clip: Or Else...</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 01:23</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>They killed her master. Now she'll kill everyone.
Welcome back to the criminal paradise of Roanapur. Fasten your seatbelts for a violent ride: mad maid Roberta is back in town with a thirst for revenge.
This tour de vio...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/haganai/dating-sims-for-loners"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/haganai/videos/promotional/dating-sims-for-loners/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556002/2_thumbnail/defghijklmnopqrstuvwxyzABCDEFGHIJ_k22734y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Dating Sims for Loners""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/haganai/anime"" class=""item-title"">Haganai</a>
<a href=""http://www.funimation.com/shows/haganai/videos/promotional/dating-sims-for-loners/anime"" class=""brief with-middle-line"">
<span>Clip</span>
Dating Sims for Loners </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/haganai/anime"" class=""item-title"">Haganai</a>
<a href=""http://www.funimation.com/shows/haganai/videos/promotional/dating-sims-for-loners/anime"" class=""item-title"">Clip - Dating Sims for Loners</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 03:19</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Sena needs to learn to make real-life friends-- and decides to pick up these skills by playing a dating sim. This can't end well.
Forever alone? Join the Club!
Yozora's an abrasive loudmouth whose only friend at school...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/guilty-crown/drawing-out-the-void"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/promotional/drawing-out-the-void/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555978/2_thumbnail/klmnopq_k13563y.png"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Drawing Out the Void""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/guilty-crown/anime"" class=""item-title"">Guilty Crown</a>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/promotional/drawing-out-the-void/anime"" class=""brief with-middle-line"">
<span>Clip</span>
Drawing Out the Void </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/guilty-crown/anime"" class=""item-title"">Guilty Crown</a>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/promotional/drawing-out-the-void/anime"" class=""item-title"">Clip - Drawing Out the Void</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 02:58</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
Shu receives a mysterious ability-- to draw out the hearts of people and use them as weapons. The beautiful songstress Inori will be his sword.</p>
<p>
Order the limited edition box set, with two gorgeous 108-page...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
</ul>
</article>
 
 
<article class=""column_2 shadow-line normal-slider flexslider"">
<div class=""basic-heading"">
<h2>TRAILERS</h2>
<a href=""http://www.funimation.com/videos/trailers"" class=""btn size1 viewAll"" data-showid="""" data-section=""trailers"">View All</a>
<img src=""http://www.funimation.com/assets/img/loading.gif"" class=""loading"" style=""display: none"">
</div>
<div>
<ul class=""items-row slides clearfix"">
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/guilty-crown/complete-series-parts-one-and-two-blu-ray-dvd-combo-limited-edition"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/promotional/complete-series-parts-one-and-two-blu-ray-dvd-combo-limited-edition/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555978/2_thumbnail/9abcdefghijklmnopqrstuvwxyzAB_k31840y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Complete Series - Parts One and Two - Blu-Ray/DVD Combo- Limited Edition""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/guilty-crown/anime"" class=""item-title"">
Guilty Crown </a>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/promotional/complete-series-parts-one-and-two-blu-ray-dvd-combo-limited-edition/anime"" class=""brief"">
Complete Series - Parts One and Two -... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/guilty-crown/anime"" class=""item-title"">Guilty Crown</a>
<a href=""http://www.funimation.com/shows/guilty-crown/videos/promotional/complete-series-parts-one-and-two-blu-ray-dvd-combo-limited-edition/anime"" class=""item-title"">Trailer - Complete Series - Parts One and Two - Blu-Ray/DVD Combo- Limited Edition</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 01:53</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
Shu&#39;s entire world was shattered after a meteorite crashed into Japan, unleashing the lethal Apocalypse Virus. The chaos and anarchy born of the outbreak cost Shu his family and reduced him to a timid, fearful s...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/fairy-tail/part-6-blu-ray-dvd-combo"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/part-6-blu-ray-dvd-combo/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555878/2_thumbnail/bcdefghijk_k44205y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Part 6- Blu-ray/DVD Combo""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/fairy-tail/anime"" class=""item-title"">
Fairy Tail </a>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/part-6-blu-ray-dvd-combo/anime"" class=""brief"">
Part 6- Blu-ray/DVD Combo </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/fairy-tail/anime"" class=""item-title"">Fairy Tail</a>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/part-6-blu-ray-dvd-combo/anime"" class=""item-title"">Trailer - Part 6- Blu-ray/DVD Combo</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 01:08</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
Nirvana continues its march to destroy Wendy&#39;s guild along with a dark secret. Team Natsu and the coalition - as well as an unexpected ally - go full-force against the top sorcerers of Oracion Seis to expel the...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/lupin-the-third-fujiko-mine/the-woman-called-fujiko-mine-blu-ray-dvd-combo-limited-edition"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/promotional/the-woman-called-fujiko-mine-blu-ray-dvd-combo-limited-edition/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556182/2_thumbnail/tuvwxyzABCDEFGHIJKL_k23406y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""The Woman Called Fujiko Mine - Blu-ray/DVD Combo – Limited Edition""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/anime"" class=""item-title"">
Lupin the Third - The Woman Called Fujiko Mine </a>
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/promotional/the-woman-called-fujiko-mine-blu-ray-dvd-combo-limited-edition/anime"" class=""brief"">
The Woman Called Fujiko Mine - Blu-ra... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/anime"" class=""item-title"">Lupin the Third - The Woman Called Fujiko Mine</a>
<a href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/promotional/the-woman-called-fujiko-mine-blu-ray-dvd-combo-limited-edition/anime"" class=""item-title"">Trailer - The Woman Called Fujiko Mine - Blu-ray/DVD Combo – Limited Edition</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 01:32</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
She&#39;s a thief. A killer. A saint and a scandal. She&#39;s whatever you need her to be to get the job done. After sizing you up with one sinful glance, she disarms you with a touch. You&#39;re powerless to resist...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/master-of-martial-hearts/complete-series"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/master-of-martial-hearts/videos/promotional/complete-series/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556198/2_thumbnail/6789abcdefghijklm_k71084y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Complete Series""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/master-of-martial-hearts/anime"" class=""item-title"">
Master of Martial Hearts </a>
<a href=""http://www.funimation.com/shows/master-of-martial-hearts/videos/promotional/complete-series/anime"" class=""brief"">
Complete Series </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/master-of-martial-hearts/anime"" class=""item-title"">Master of Martial Hearts</a>
<a href=""http://www.funimation.com/shows/master-of-martial-hearts/videos/promotional/complete-series/anime"" class=""item-title"">Trailer - Complete Series</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 01:07</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Aya Iseshima's carefree existence takes a bone-crushingly violent turn when she wanders into the midst of a sadistic, girl-on-girl martial arts tournament. Her freshly pressed school uniform doesn't stand a chance of sur...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/black-lagoon/roberta-s-blood-trail-ova-blu-ray-dvd-combo"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/black-lagoon/videos/promotional/roberta-s-blood-trail-ova-blu-ray-dvd-combo/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555674/2_thumbnail/defghijklmnopqrstuvwxyzABCDEFGHIJKLM_k50553y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Roberta's Blood Trail OVA - Blu-ray/DVD Combo""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/black-lagoon/anime"" class=""item-title"">
Black Lagoon </a>
<a href=""http://www.funimation.com/shows/black-lagoon/videos/promotional/roberta-s-blood-trail-ova-blu-ray-dvd-combo/anime"" class=""brief"">
Roberta's Blood Trail OVA - Blu-ray/D... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/black-lagoon/anime"" class=""item-title"">Black Lagoon</a>
<a href=""http://www.funimation.com/shows/black-lagoon/videos/promotional/roberta-s-blood-trail-ova-blu-ray-dvd-combo/anime"" class=""item-title"">Trailer - Roberta's Blood Trail OVA - Blu-ray/DVD Combo</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 01:15</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
Deadly assassin-turned-maid Roberta returns to the criminal&#39;s paradise of Roanapur, dead set on revenge - no matter the cost. In a backdrop of violent chaos, the smugglers of the Lagoon Company team up with Robe...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/lastexile-fam-the-silver-wing/parts-one-and-two-blu-ray-dvd-combo-limited-edition"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/videos/promotional/parts-one-and-two-blu-ray-dvd-combo-limited-edition/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556154/2_thumbnail/hijklmnopqrs_k4237y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Parts One and Two - Blu-ray/DVD Combo - Limited Edition""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/anime"" class=""item-title"">
LASTEXILE -Fam, the Silver Wing </a>
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/videos/promotional/parts-one-and-two-blu-ray-dvd-combo-limited-edition/anime"" class=""brief"">
Parts One and Two - Blu-ray/DVD Combo... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/anime"" class=""item-title"">LASTEXILE -Fam, the Silver Wing</a>
<a href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/videos/promotional/parts-one-and-two-blu-ray-dvd-combo-limited-edition/anime"" class=""item-title"">Trailer - Parts One and Two - Blu-ray/DVD Combo - Limited Edition</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 01:49</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
Years ago, humanity abandoned the ruined Earth. Generations later, with the planet again capable of sustaining life, mankind returned. In the skies above the reborn world, rebellious young Fam and her best friend Gi...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/the-future-diary/part-one-limited-edition"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/the-future-diary/videos/promotional/part-one-limited-edition/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556582/2_thumbnail/9abcdefghijklmnopqrstuvwxyzABCDEFGHI_k14981y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Part One - Limited Edition""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/the-future-diary/anime"" class=""item-title"">
The Future Diary </a>
<a href=""http://www.funimation.com/shows/the-future-diary/videos/promotional/part-one-limited-edition/anime"" class=""brief"">
Part One - Limited Edition </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/the-future-diary/anime"" class=""item-title"">The Future Diary</a>
<a href=""http://www.funimation.com/shows/the-future-diary/videos/promotional/part-one-limited-edition/anime"" class=""item-title"">Trailer - Part One - Limited Edition</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 01:31</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
One day, Yukiteru discovers that his cell phone &quot;diary&quot; can now tell him the events of the future. The problem is, eleven others also have similar diaries, and only one can win this Survival Game. The winn...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/one-piece/season-five-voyage-two"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/one-piece/videos/promotional/season-five-voyage-two/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556282/2_thumbnail/ij_k96377y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Season Five Voyage Two""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/one-piece/anime"" class=""item-title"">
One Piece </a>
<a href=""http://www.funimation.com/shows/one-piece/videos/promotional/season-five-voyage-two/anime"" class=""brief"">
Season Five Voyage Two </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/one-piece/anime"" class=""item-title"">One Piece</a>
<a href=""http://www.funimation.com/shows/one-piece/videos/promotional/season-five-voyage-two/anime"" class=""item-title"">Trailer - Season Five Voyage Two</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 01:16</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
Monkey D. Luffy refuses to let anyone or anything stand in the way of his quest to become King of All Pirates. With a course charted for the treacherous waters of the Grand Line, this is one captain who&#39;ll never...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/sankarea/complete-series-blu-ray-dvd-combo-limited-edition"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/sankarea/videos/promotional/complete-series-blu-ray-dvd-combo-limited-edition/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556406/2_thumbnail/hijklmnopqrstuvwxyzABCDEFGHIJ_k61955y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Complete Series - Blu-ray/DVD Combo - Limited Edition""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/sankarea/anime"" class=""item-title"">
Sankarea </a>
<a href=""http://www.funimation.com/shows/sankarea/videos/promotional/complete-series-blu-ray-dvd-combo-limited-edition/anime"" class=""brief"">
Complete Series - Blu-ray/DVD Combo -... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/sankarea/anime"" class=""item-title"">Sankarea</a>
<a href=""http://www.funimation.com/shows/sankarea/videos/promotional/complete-series-blu-ray-dvd-combo-limited-edition/anime"" class=""item-title"">Trailer - Complete Series - Blu-ray/DVD Combo - Limited Edition</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 01:09</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
Furuya&#39;s not interested in the living, he&#39;s got zombies on the brain! When Furuya&#39;s cat dies, he decides he&#39;s going to try and bring it back to life. In the process, he stumbles across a girl whose f...</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/we-without-wings/official-redband-trailer"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/we-without-wings/videos/promotional/official-redband-trailer/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556674/2_thumbnail/jklmnopqrstuvwxy_k37803y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Official Redband Trailer""/>
</a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/we-without-wings/anime"" class=""item-title"">
We Without Wings </a>
<a href=""http://www.funimation.com/shows/we-without-wings/videos/promotional/official-redband-trailer/anime"" class=""brief"">
Official Redband Trailer </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/we-without-wings/anime"" class=""item-title"">We Without Wings</a>
<a href=""http://www.funimation.com/shows/we-without-wings/videos/promotional/official-redband-trailer/anime"" class=""item-title"">Trailer - Official Redband Trailer</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 01:35</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
We Without Wings Official Redband Trailer</p></p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
</ul>
</div>
</article>
 
<article class=""column_2 shadow-line normal-slider flexslider"">
<div class=""basic-heading"">
<h2>EXTRAS</h2>
<a href=""http://www.funimation.com/videos/extras"" class=""btn size1 viewAll"" data-showid="""" data-section=""extras"">View All</a>
<img src=""http://www.funimation.com/assets/img/loading.gif"" class=""loading"" style=""display: none"">
</div>
<div>
<ul class=""items-row slides clearfix"">
<li class=""item-cell pop elite simulcast"">
<div href=""#"" class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/fullmetal-alchemist/english-voice-actor-commentary"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/fullmetal-alchemist/videos/promotional/english-voice-actor-commentary/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555918/2_thumbnail/6789abcdefghijklmnopqrstuvw_k78103y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""English Voice Actor Commentary""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/fullmetal-alchemist/anime"" class=""item-title"">Fullmetal Alchemist</a>
<a href=""http://www.funimation.com/shows/fullmetal-alchemist/videos/promotional/english-voice-actor-commentary/anime"" class=""brief with-middle-line"">
<span>Commentary</span>
English Voice Actor Commentary </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/fullmetal-alchemist/anime"" class=""item-title"">Fullmetal Alchemist</a>
<a href=""http://www.funimation.com/shows/fullmetal-alchemist/videos/promotional/english-voice-actor-commentary/anime"" class=""item-title"">Commentary - English Voice Actor Commentary</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 01:45:04</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Voice Actor commentary for Fullmetal Alchemist: Conqueror of Shamballa</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div href=""#"" class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/flcl/ride-on-shooting-star-video-by-the-pillows"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/flcl/videos/promotional/ride-on-shooting-star-video-by-the-pillows/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555890/2_thumbnail/cdefghijklmnopqrstuvwxyzABC_k79711y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Ride on Shooting Star Video by The Pillows""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/flcl/anime"" class=""item-title"">FLCL</a>
<a href=""http://www.funimation.com/shows/flcl/videos/promotional/ride-on-shooting-star-video-by-the-pillows/anime"" class=""brief with-middle-line"">
<span>Extra Feature</span>
Ride on Shooting Star Video by The Pi... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/flcl/anime"" class=""item-title"">FLCL</a>
<a href=""http://www.funimation.com/shows/flcl/videos/promotional/ride-on-shooting-star-video-by-the-pillows/anime"" class=""item-title"">Extra Feature - Ride on Shooting Star Video by The Pillows</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 02:32</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Ride on Shooting Star Video by The Pillows</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div href=""#"" class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/ouran-high-school-host-club/outtakes-part-1"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/promotional/outtakes-part-1/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556294/2_thumbnail/qrstuvwxyzABCDEFGHIJ_k40646y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Outtakes Part 1""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/anime"" class=""item-title"">Ouran High School Host Club</a>
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/promotional/outtakes-part-1/anime"" class=""brief with-middle-line"">
<span>Extra Feature</span>
Outtakes Part 1 </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/anime"" class=""item-title"">Ouran High School Host Club</a>
<a href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/promotional/outtakes-part-1/anime"" class=""item-title"">Extra Feature - Outtakes Part 1</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 15:11</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
N/A</p></p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div href=""#"" class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/shin-chan/season-1-from-the-bowels-of-the-booth-1"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/shin-chan/videos/promotional/season-1-from-the-bowels-of-the-booth-1/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556462/2_thumbnail/456789abcdefghijklmnopqrstuvwxyzABCDEFGH_k34859y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Season 1 - From the Bowels of the Booth 1""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/shin-chan/anime"" class=""item-title"">Shin chan</a>
<a href=""http://www.funimation.com/shows/shin-chan/videos/promotional/season-1-from-the-bowels-of-the-booth-1/anime"" class=""brief with-middle-line"">
<span>Extra Feature</span>
Season 1 - From the Bowels of the Boo... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/shin-chan/anime"" class=""item-title"">Shin chan</a>
<a href=""http://www.funimation.com/shows/shin-chan/videos/promotional/season-1-from-the-bowels-of-the-booth-1/anime"" class=""item-title"">Extra Feature - Season 1 - From the Bowels of the Booth 1</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 11:08</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
<span class=""field-content"">&nbsp;Alternate/Deleted Scenes, Bloopers</span></p></p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div href=""#"" class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/soul-eater/episode-7-voice-actor-commentary"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/soul-eater/videos/promotional/episode-7-voice-actor-commentary/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7556490/2_thumbnail/tuvwxyzABCDEFGH_k27270y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Episode 7 Voice Actor Commentary""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/soul-eater/anime"" class=""item-title"">Soul Eater</a>
<a href=""http://www.funimation.com/shows/soul-eater/videos/promotional/episode-7-voice-actor-commentary/anime"" class=""brief with-middle-line"">
<span>Commentary</span>
Episode 7 Voice Actor Commentary </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/soul-eater/anime"" class=""item-title"">Soul Eater</a>
<a href=""http://www.funimation.com/shows/soul-eater/videos/promotional/episode-7-voice-actor-commentary/anime"" class=""item-title"">Commentary - Episode 7 Voice Actor Commentary</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 23:50</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p><p>
<span class=""field-content"">Episode 7 Commentary (Zach Bolton &ndash; ADR Director, Laura Bailey - Maka, Micah Solusod - Soul)</span></p></p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div href=""#"" class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/fairy-tail/textless-opening-part-1"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/textless-opening-part-1/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555878/2_thumbnail/ijklmnopqrstuvwxyzABCDE_k66236y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Textless Opening Part 1""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/fairy-tail/anime"" class=""item-title"">Fairy Tail</a>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/textless-opening-part-1/anime"" class=""brief with-middle-line"">
<span>Extra Feature</span>
Textless Opening Part 1 </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/fairy-tail/anime"" class=""item-title"">Fairy Tail</a>
<a href=""http://www.funimation.com/shows/fairy-tail/videos/promotional/textless-opening-part-1/anime"" class=""item-title"">Extra Feature - Textless Opening Part 1</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-14 | 01:30</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>Part 1 Opening Song - SNOW FAIRY</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div href=""#"" class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/fruits-basket/fruits-basket-voice-actor-video-commentary"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/fruits-basket/videos/promotional/fruits-basket-voice-actor-video-commentary/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555902/2_thumbnail/lmnopqrstuvwxyzABCDE_k3989y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Fruits Basket Voice Actor Video Commentary""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/fruits-basket/anime"" class=""item-title"">Fruits Basket</a>
<a href=""http://www.funimation.com/shows/fruits-basket/videos/promotional/fruits-basket-voice-actor-video-commentary/anime"" class=""brief with-middle-line"">
<span>Commentary</span>
Fruits Basket Voice Actor Video Comme... </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/fruits-basket/anime"" class=""item-title"">Fruits Basket</a>
<a href=""http://www.funimation.com/shows/fruits-basket/videos/promotional/fruits-basket-voice-actor-video-commentary/anime"" class=""item-title"">Commentary - Fruits Basket Voice Actor Video Commentary</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-PG | 23:19</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>We sat down with Laura Bailey (Tohru), Eric Vale (Yuki), Jerry Jewell (Kyo), and John Burgmeier (Shigure) to reminisce about their work on Fruits Basket, what recording was like, and what they’ve been up to since then</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
<li class=""item-cell pop elite simulcast"">
<div href=""#"" class=""thumb clearfix"">
<div class=""bottom-icons"">
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-heart fancybox fancybox.iframe"" title=""Add to Favorites""></a>
<a href=""http://www.funimation.com/loadModal/index/join_now_modal"" data-fancybox-width=""500"" class=""icon-plus fancybox fancybox.iframe"" title=""Add to Queue""></a>
<a href=""http://www.funimation.com/loadModal/index/share_modal_video/basilisk/cast-audition"" class=""icon-share fancybox fancybox.iframe"" data-fancybox-width=""630"" title=""Share This""></a>
</div>
<a href=""http://www.funimation.com/shows/basilisk/videos/promotional/cast-audition/anime"">
<img data-original=""http://www.funimation.com/admin/uploads/default/promo_thumnails/7555646/2_thumbnail/3456789abcdefghijklmnopqrstuvwxyzABCD_k79958y.jpg"" src=""http://www.funimation.com/admin/uploads/default/default_images/2_thumbnail/VideoGuide.jpg"" border=""0"" width=""140"" height=""78"" alt=""Cast Audition""/></a>
</div>
<div class=""item-resume-info clearfix"">
<a href=""http://www.funimation.com/shows/basilisk/anime"" class=""item-title"">Basilisk</a>
<a href=""http://www.funimation.com/shows/basilisk/videos/promotional/cast-audition/anime"" class=""brief with-middle-line"">
<span>Extra Feature</span>
Cast Audition </a>
</div>
 
<div class=""hide"">
<div class=""popup-heading"">
<a href=""http://www.funimation.com/shows/basilisk/anime"" class=""item-title"">Basilisk</a>
<a href=""http://www.funimation.com/shows/basilisk/videos/promotional/cast-audition/anime"" class=""item-title"">Extra Feature - Cast Audition</a>
</div>
<div class=""item-popup"">
<div class=""pop-content"">
<div class=""clearfix"">
<p class=""left"">TV-MA | 23:52</p>
<div class=""rank rank-5-stars right"">
<ul>
<li class=""star-one""></li>
<li class=""star-two""></li>
<li class=""star-three""></li>
<li class=""star-four""></li>
<li class=""star-five""></li>
</ul>
</div>
</div>
<p>US Cast Auditions (Gennosuke, Oboro, Akeginu, Kazamachi, Tenzen, Roudai, Ieyasu, Yashamaru, Saemon, Udono)</p>
<p>
<a href=""http://www.funimation.com/subscribe/anime"">Subscribe to watch episodes in HD commercial-free.</a>
</p>
</div>
</div>
</div>
  </li>
</ul>
</div>
</article>
 
</section>
<aside>
 
<div class=""column_1 shadow-line sidebar-banner clearfix"">
 
<div id='div-gpt-ad-1374328695293-0' class=""sidebar-banner"">
<script type='text/rocketscript'>
            googletag.cmd.push(function() { googletag.display('div-gpt-ad-1374328695293-0'); });
        </script>
</div>
</div>
   
<article class=""share-funi-page column_1 shadow-line share four-items"">
<div class=""basic-heading"">
<h2>Share This</h2>
</div>
<ul>
<li class=""t-center"">
 
<a href=""#"" target=""_blank"" class=""google-count-urlh"">
<span class=""t-center black foundicon-google-plus""></span>
</a>
</li>
<li class=""t-center"">
 
<a href=""#"" target=""_blank"" class=""twitter-count-urlh"">
<span class=""t-center black foundicon-twitter""></span>
</a>
</li>
<li class=""t-center"">
 
<a href=""#"" target=""_blank"" class=""facebook-count-urlh"">
<span class=""t-center black foundicon-facebook""></span>
</a>
</li>
<li class=""t-center"">
 
<a href=""#"" target=""_blank"" class=""tumblr-count-urlh"">
<span class=""t-center black foundicon-tumblr""></span>
</a>
</li>
 
 
 
 
 
 
</ul>
</article>
<script type=""text/rocketscript"">
	head.ready(function() {

        var page = 'videos';
	    shareThis(document.URL);
	    $('.tumblr-count-urlh').on('click',function(){ addParamTumblrWidgetLink(document.URL,document.title,'');});
		$('.facebook-count-urlh').on('click',function(){addParamFacebookWidget( page );});
		$('.twitter-count-urlh').on('click',function(event){addParamTwitterWidget(event);});
		$('.google-count-urlh').on('click',function(){addParamGooglePlusWidget( page );});
	});
</script>
 
 
<article class=""column_1 shadow-line table-type1"">
<div class=""basic-heading"">
<h2>WATCH FULL EPISODES</h2>
</div>
<ul class=""links-list"">
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/hackquantum/videos/episodes/anime"">.hack//Quantum</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/a-certain-magical-index/videos/episodes/anime"">A Certain Magical Index</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/a-certain-scientific-railgun/videos/episodes/anime"">A Certain Scientific Railgun</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/aesthetica-of-a-rogue-hero/videos/episodes/anime"">Aesthetica of a Rogue Hero</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/afro-samurai/videos/episodes/anime"">Afro Samurai</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ah-my-goddess-flights-of-fancy/videos/episodes/anime"">Ah! My Goddess: Flights of Fancy</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ai-yori-aoshi/videos/episodes/anime"">Ai Yori Aoshi</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/air/videos/episodes/anime"">Air</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/air-gear/videos/episodes/anime"">Air Gear</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/air-master/videos/episodes/anime"">Air Master</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/appleseed-xiii/videos/episodes/anime"">Appleseed XIII</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/aquarion/videos/episodes/anime"">Aquarion</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/aquarion-evol/videos/episodes/anime"">Aquarion EVOL</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/aria-the-scarlet-ammo/videos/episodes/anime"">Aria the Scarlet Ammo</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/armitage-iii/videos/episodes/anime"">Armitage III</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/attack-on-titan/videos/episodes/anime"">Attack on Titan</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/baccano/videos/episodes/anime"">Baccano!</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/baka-and-test-summon-the-beasts/videos/episodes/anime"">Baka & Test - Summon the Beasts -</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/baldr-force-exe/videos/episodes/anime"">Baldr Force Exe</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/bamboo-blade/videos/episodes/anime"">Bamboo Blade</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/basilisk/videos/episodes/anime"">Basilisk</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/big-windup/videos/episodes/anime"">Big Windup!</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/birdy-the-mighty-decode/videos/episodes/anime"">Birdy the Mighty: Decode</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/black-blood-brothers/videos/episodes/anime"">Black Blood Brothers</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/black-butler/videos/episodes/anime"">Black Butler</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/black-cat/videos/episodes/anime"">Black Cat</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/black-lagoon/videos/episodes/anime"">Black Lagoon</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/blassreiter/videos/episodes/anime"">Blassreiter</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/blazblue/videos/episodes/anime"">BlazBlue: Alter Memory</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/blessing-of-the-campanella/videos/episodes/anime"">Blessing of the Campanella</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/blood-c/videos/episodes/anime"">BLOOD-C</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/blue-gender/videos/episodes/anime"">Blue Gender</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/brothers-conflict/videos/episodes/anime"">Brothers Conflict</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/bubblegum-crisis-tokyo-2040/videos/episodes/anime"">Bubblegum Crisis: Tokyo 2040</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/burst-angel/videos/episodes/anime"">Burst Angel</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/c-control/videos/episodes/anime"">C - Control</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/c3-anime/videos/episodes/anime"">C3 Anime</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/captain-harlock/videos/episodes/anime"">Captain Harlock</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/case-closed/videos/episodes/anime"">Case Closed</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/casshern-sins/videos/episodes/anime"">Casshern Sins</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/cat-planet-cuties/videos/episodes/anime"">Cat Planet Cuties</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/chaos-head/videos/episodes/anime"">Chaos;HEAd</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/chobits/videos/episodes/anime"">Chobits</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/chrome-shelled-regios/videos/episodes/anime"">Chrome Shelled Regios</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/chrono-crusade/videos/episodes/anime"">Chrono Crusade</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/claymore/videos/episodes/anime"">Claymore</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/code-breaker/videos/episodes/anime"">Code:Breaker</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/comic-party-revolution/videos/episodes/anime"">Comic Party Revolution</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/corpse-princess-shikabane-hime/videos/episodes/anime"">Corpse Princess: Shikabane Hime</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/coyote-ragtime-show/videos/episodes/anime"">Coyote Ragtime Show</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/dgray-man/videos/episodes/anime"">D.Gray-man</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/dance-in-the-vampire-bund/videos/episodes/anime"">Dance in the Vampire Bund</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/danganronpa-the-animation/videos/episodes/anime"">Danganronpa: The Animation</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/darker-than-black/videos/episodes/anime"">Darker Than Black</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/date-a-live/videos/episodes/anime"">Date A Live</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/deadman-wonderland/videos/episodes/anime"">Deadman Wonderland</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/desert-punk/videos/episodes/anime"">Desert Punk</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/detective-opera-milky-holmes-2/videos/episodes/anime"">Detective Opera Milky Holmes 2</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/devil-may-cry/videos/episodes/anime"">Devil May Cry</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/digimon-adventure-02/videos/episodes/anime"">Digimon Adventure 02</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/digimon-tamers/videos/episodes/anime"">Digimon Tamers</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/disgaea/videos/episodes/anime"">Disgaea</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/dragon-ball/videos/episodes/anime"">Dragon Ball</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/dragon-ball-z/videos/episodes/anime"">Dragon Ball Z</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/dragonaut-the-resonance/videos/episodes/anime"">Dragonaut -THE RESONANCE-</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/eden-of-the-east/videos/episodes/anime"">Eden of the East</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/el-cazador-de-la-bruja/videos/episodes/anime"">El Cazador de la Bruja</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ergo-proxy/videos/episodes/anime"">Ergo Proxy</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/eureka-seven-astral-ocean/videos/episodes/anime"">Eureka Seven AO</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/excel-saga/videos/episodes/anime"">Excel Saga</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/fafner/videos/episodes/anime"">Fafner</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/fairy-tail/videos/episodes/anime"">Fairy Tail</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/fist-of-the-north-star/videos/episodes/anime"">Fist of the North Star</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/flcl/videos/episodes/anime"">FLCL</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/fractale/videos/episodes/anime"">Fractale</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/freezing/videos/episodes/anime"">Freezing</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/fruits-basket/videos/episodes/anime"">Fruits Basket</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/full-metal-panic/videos/episodes/anime"">Full Metal Panic!</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/full-metal-panic-the-second-raid/videos/episodes/anime"">Full Metal Panic! The Second Raid</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/full-metal-panic-fumoffu/videos/episodes/anime"">Full Metal Panic? Fumoffu</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/fullmetal-alchemist/videos/episodes/anime"">Fullmetal Alchemist</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/fullmetal-alchemist-brotherhood/videos/episodes/anime"">Fullmetal Alchemist: Brotherhood</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ga-rei-zero/videos/episodes/anime"">Ga-Rei-Zero</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/gad-guard/videos/episodes/anime"">Gad Guard</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/gaiking/videos/episodes/anime"">Gaiking</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/galaxy-express-999/videos/episodes/anime"">Galaxy Express 999</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ghost-hunt/videos/episodes/anime"">Ghost Hunt</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/girls-bravo/videos/episodes/anime"">Girls Bravo</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/glass-fleet/videos/episodes/anime"">Glass Fleet</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/good-luck-girl/videos/episodes/anime"">Good Luck Girl</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/guilty-crown/videos/episodes/anime"">Guilty Crown</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/gun-x-sword/videos/episodes/anime"">Gun X Sword</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/gungrave/videos/episodes/anime"">Gungrave</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/gunslinger-girl/videos/episodes/anime"">Gunslinger Girl</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/gunslinger-girl-il-teatrino/videos/episodes/anime"">Gunslinger Girl - Il Teatrino</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/guyver-the-bioboosted-armor/videos/episodes/anime"">Guyver: The Bioboosted Armor</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/haganai/videos/episodes/anime"">Haganai</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/haibane-renmei/videos/episodes/anime"">Haibane Renmei</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/heat-guy-j/videos/episodes/anime"">Heat Guy J</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/heavens-lost-property/videos/episodes/anime"">Heaven's Lost Property</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/hellsing/videos/episodes/anime"">Hellsing</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/hellsing-ultimate/videos/episodes/anime"">Hellsing Ultimate</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/hero-tales/videos/episodes/anime"">Hero Tales</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/heroic-age/videos/episodes/anime"">Heroic Age</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/hetalia/videos/episodes/anime"">Hetalia</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/high-school-dxd/videos/episodes/anime"">High School DxD</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/house-of-five-leaves/videos/episodes/anime"">House of Five Leaves</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/hyperdimension-neptunia/videos/episodes/anime"">Hyperdimension Neptunia</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ikki-tousen/videos/episodes/anime"">Ikki Tousen</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/initial-d/videos/episodes/anime"">Initial D</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/inside-the-voice-actors-studio/videos/episodes/anime"">Inside The Voice Actors Studio</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/interlude/videos/episodes/anime"">Interlude</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/is-this-a-zombie/videos/episodes/anime"">Is this a Zombie?</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/jing-king-of-bandits/videos/episodes/anime"">Jing King of Bandits</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/jinki-extend/videos/episodes/anime"">Jinki: Extend</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/jormungand/videos/episodes/anime"">Jormungand</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/jyu-oh-sei/videos/episodes/anime"">Jyu-Oh-Sei</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/kaleido-star/videos/episodes/anime"">Kaleido Star</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/kamisama-kiss/videos/episodes/anime"">Kamisama Kiss</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/kanon/videos/episodes/anime"">Kanon</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/karneval/videos/episodes/anime"">Karneval</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/kaze-no-stigma/videos/episodes/anime"">Kaze No Stigma</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/kenichi-the-mightiest-disciple/videos/episodes/anime"">Kenichi: The Mightiest Disciple</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/kiddy-grade/videos/episodes/anime"">Kiddy Grade</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/kingdom/videos/episodes/anime"">Kingdom</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/kurau-phantom-memory/videos/episodes/anime"">Kurau Phantom Memory</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/last-exile/videos/episodes/anime"">Last Exile</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/lastexile-fam-the-silver-wing/videos/episodes/anime"">LASTEXILE -Fam, the Silver Wing</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/le-chevalier-deon/videos/episodes/anime"">Le Chevalier D'Eon</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/level-e/videos/episodes/anime"">Level E</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/linebarrels-of-iron/videos/episodes/anime"">Linebarrels of Iron</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/lupin-the-third-fujiko-mine/videos/episodes/anime"">Lupin the Third - The Woman Called Fujiko Mine</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/magikano/videos/episodes/anime"">Magikano</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/maken-ki/videos/episodes/anime"">Maken-ki!</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/master-of-martial-hearts/videos/episodes/anime"">Master of Martial Hearts</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/michiko-hatchin/videos/episodes/anime"">Michiko & Hatchin</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/minami-ke/videos/episodes/anime"">Minami-ke</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/mongolian-chop-squad/videos/episodes/anime"">Mongolian Chop Squad</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/moonphase/videos/episodes/anime"">MoonPhase</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/moyashimon-tales-of-agriculture/videos/episodes/anime"">Moyashimon: Tales of Agriculture</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/murder-princess/videos/episodes/anime"">Murder Princess</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/mushi-shi/videos/episodes/anime"">Mushi-Shi</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/my-bride-is-a-mermaid/videos/episodes/anime"">My Bride is a Mermaid</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/nabari-no-ou/videos/episodes/anime"">Nabari no Ou</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/negima-magister-negi-magi/videos/episodes/anime"">Negima!? Magister Negi Magi</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/noir/videos/episodes/anime"">Noir</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/oh-edo-rocket/videos/episodes/anime"">Oh! Edo Rocket</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/okami-san-and-her-seven-companions/videos/episodes/anime"">Okami-san and Her Seven Companions</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/one-piece/videos/episodes/anime"">One Piece</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/oniai/videos/episodes/anime"">OniAi</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ouran-high-school-host-club/videos/episodes/anime"">Ouran High School Host Club</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/pani-poni-dash/videos/episodes/anime"">Pani Poni Dash!</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/panty-stocking-with-garterbelt/videos/episodes/anime"">Panty & Stocking with Garterbelt</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/peacemaker/videos/episodes/anime"">Peacemaker</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/phantom-requiem-for-the-phantom/videos/episodes/anime"">Phantom: Requiem for the Phantom</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/pretear/videos/episodes/anime"">Pretear</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/pretty-cure/videos/episodes/anime"">Pretty Cure</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/princess-jellyfish/videos/episodes/anime"">Princess Jellyfish</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/project-blue-earth-sos/videos/episodes/anime"">Project Blue Earth SOS</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/psycho-pass/videos/episodes/anime"">PSYCHO-PASS</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/puchims/videos/episodes/anime"">PUCHIM@S</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/pumpkin-scissors/videos/episodes/anime"">Pumpkin Scissors</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ragnarok-the-animation/videos/episodes/anime"">Ragnarok - The Animation</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/red-data-girl/videos/episodes/anime"">Red Data Girl</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/red-garden/videos/episodes/anime"">Red Garden</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/rideback/videos/episodes/anime"">RideBack</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/rin-daughters-of-mnemosyne/videos/episodes/anime"">RIN ~Daughters of Mnemosyne~</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/roboticsnotes/videos/episodes/anime"">Robotics;Notes</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/romeo-x-juliet/videos/episodes/anime"">Romeo x Juliet</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/rosario-vampire/videos/episodes/anime"">Rosario + Vampire</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/rumbling-hearts/videos/episodes/anime"">Rumbling Hearts</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/samurai-7/videos/episodes/anime"">Samurai 7</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/samurai-champloo/videos/episodes/anime"">Samurai Champloo</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/sands-of-destruction/videos/episodes/anime"">Sands of Destruction</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/sankarea/videos/episodes/anime"">Sankarea</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/sasami-magical-girls-club/videos/episodes/anime"">Sasami Magical Girls Club</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/save-me-lollipop/videos/episodes/anime"">Save Me! Lollipop</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/school-rumble/videos/episodes/anime"">School Rumble</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/sekirei/videos/episodes/anime"">Sekirei</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/sengoku-basara-samurai-kings/videos/episodes/anime"">Sengoku BASARA: Samurai Kings</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/senran-kagura/videos/episodes/anime"">Senran Kagura</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/serial-experiments-lain/videos/episodes/anime"">Serial Experiments Lain</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/sgt-frog/videos/episodes/anime"">Sgt. Frog</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/shakugan-no-shana/videos/episodes/anime"">Shakugan No Shana</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/shangri-la/videos/episodes/anime"">Shangri-la</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/shattered-angels/videos/episodes/anime"">Shattered Angels</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/shigurui-death-frenzy/videos/episodes/anime"">Shigurui: Death Frenzy</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/shiki/videos/episodes/anime"">Shiki</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/shin-chan/videos/episodes/anime"">Shin chan</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/shuffle/videos/episodes/anime"">Shuffle!</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/slam-dunk/videos/episodes/anime"">Slam Dunk</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/solty-rei/videos/episodes/anime"">Solty Rei</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/soul-eater/videos/episodes/anime"">Soul Eater</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/speed-grapher/videos/episodes/anime"">Speed Grapher</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/spice-and-wolf/videos/episodes/anime"">Spice and Wolf</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/steins-gate/videos/episodes/anime"">Steins;Gate</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/strain-strategic-armored-infantry/videos/episodes/anime"">STRAIN: Strategic Armored Infantry</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/strike-witches/videos/episodes/anime"">Strike Witches</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/suzuka/videos/episodes/anime"">Suzuka</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/symphogear/videos/episodes/anime"">Symphogear</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tenchi-muyo-gxp/videos/episodes/anime"">Tenchi Muyo! GXP</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tenchi-muyo-ova-series/videos/episodes/anime"">Tenchi Muyo! OVA Series</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tenchi-muyo-ryo-ohki/videos/episodes/anime"">Tenchi Muyo! Ryo-Ohki</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tenchi-muyo-tenchi-in-tokyo/videos/episodes/anime"">Tenchi Muyo! Tenchi in Tokyo</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tenchi-muyo-tenchi-universe/videos/episodes/anime"">Tenchi Muyo! Tenchi Universe</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tenchi-muyo-war-on-geminar/videos/episodes/anime"">Tenchi Muyo! War on Geminar</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/texhnolyze/videos/episodes/anime"">Texhnolyze</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-count-of-monte-cristo-gankutsuou/videos/episodes/anime"">The Count of Monte Cristo: Gankutsuou</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-devil-is-a-part-timer/videos/episodes/anime"">The Devil is a Part-Timer!</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-funimation-show/videos/episodes/anime"">The FUNimation Show</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-future-diary/videos/episodes/anime"">The Future Diary</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-galaxy-railways/videos/episodes/anime"">The Galaxy Railways</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-legend-of-the-legendary-heroes/videos/episodes/anime"">The Legend of the Legendary Heroes</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-sacred-blacksmith/videos/episodes/anime"">The Sacred Blacksmith</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-slayers/videos/episodes/anime"">The Slayers</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-slayers-revolution/videos/episodes/anime"">The Slayers Revolution</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-tatami-galaxy/videos/episodes/anime"">The Tatami Galaxy</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/the-wallflower/videos/episodes/anime"">The Wallflower</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/to-movie/videos/episodes/anime"">TO Movie</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tokyo-majin/videos/episodes/anime"">Tokyo Majin</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tokyo-ravens/videos/episodes/anime"">Tokyo Ravens</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/toriko/videos/episodes/anime"">Toriko</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tower-of-druaga/videos/episodes/anime"">Tower of Druaga</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/trigun/videos/episodes/anime"">Trigun</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/trinity-blood/videos/episodes/anime"">Trinity Blood</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/tsubasa-reservoir-chronicle/videos/episodes/anime"">Tsubasa RESERVoir CHRoNiCLE</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/ufo-ultramaiden-valkyrie/videos/episodes/anime"">UFO Ultramaiden Valkyrie</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/unbreakable-machine-doll/videos/episodes/anime"">Unbreakable Machine-Doll</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/utawarerumono/videos/episodes/anime"">Utawarerumono</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/vandread/videos/episodes/anime"">Vandread</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/venus-versus-virus/videos/episodes/anime"">Venus Versus Virus</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/we-without-wings/videos/episodes/anime"">We Without Wings</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/welcome-to-the-n-h-k/videos/episodes/anime"">Welcome to the N-H-K</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/witchblade/videos/episodes/anime"">Witchblade</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/x/videos/episodes/anime"">X</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/xxxholic/videos/episodes/anime"">xxxHOLiC</a></li>
<li><a class=""fs16 bold"" href=""http://www.funimation.com/shows/yamadas-first-time-b-gata-h-kei/videos/episodes/anime"">Yamada's First Time: B Gata H Kei</a></li>
</ul>
</article>
 
 
<div class=""column_1 shadow-line normal-slider with-tabs dvd-bd-schedule"">
<div class=""basic-heading"">
<h2>COMING SOON</h2>
 
</div>
 
<article class=""normal-slider flexslider"">
<div class=""normal-slider-with-tabs"">
<ul>
<li>
<div class=""basic-heading""><a href=""#normal-slider-with-tabs-1"">
<h2>ELITE</h2>
</a><a href=""http://www.funimation.com/schedule/subscription"" class=""btn size1"">View All</a></div>
</li>
<li>
<div class=""basic-heading""><a href=""#normal-slider-with-tabs-2"">
<h2>FREE</h2>
</a><a href=""http://www.funimation.com/schedule/streaming"" class=""btn size1"">View All</a></div>
</li>
</ul>
<div id=""normal-slider-with-tabs-1"">
</div>
<div id=""normal-slider-with-tabs-2"">
</div>
</div>
</article>
</div>
  </aside>
</section>
<footer class=""clear"">
<section class=""center-layout"">
<article class=""main-footer left"">
<ul class=""clearfix"">
 
<li class=""left""><a href=""http://www.funimation.com/account""><span>Account</span></a></li>
<li class=""left""><a href=""http://www.funimation.com/support""><span>Help</span></a></li>
<li class=""left""><a href=""http://www.funimation.com/about-us/anime""><span>About Us</span></a></li>
<li class=""left""><a href=""http://www.funimation.com/return-policy/anime""><span>Policies</span></a></li>
 
<li class=""left""><a href=""http://www.funimation.com/terms-of-use/anime""><span>Terms</span></a></li>
<li class=""left""><a href=""http://www.funimation.com/privacy-policy/anime""><span>Privacy</span></a></li>
</ul>
<span class=""copyright show"">&copy; 2014 FUNimation Productions, LTD. All Rights Reserved.</span>
</article>
<article class=""social-footer right"">
<ul class=""clearfix"">
<li class=""left fs14""><span class=""bold"">Follow us:</span></li>
<li class=""left""><a href=""http://www.facebook.com/funimation"" target=""_blank""><span class=""social foundicon-facebook""></span></a></li>
<li class=""left""><a href=""http://www.twitter.com/funimation"" target=""_blank""><span class=""social foundicon-twitter""></span></a></li>
<li class=""left""><a href=""https://plus.google.com/+FUNimation"" target=""_blank""><span class=""social foundicon-google-plus""></span></a></li>
<li class=""left""><a href=""https://www.youtube.com/funimation"" target=""_blank""><span class=""social foundicon-youtube""></span></a></li>
<li class=""left""><a href=""http://www.pinterest.com/funimation"" target=""_blank""><span class=""social foundicon-pinterest""></span></a></li>
<li class=""left""><a href=""http://funimation.tumblr.com"" target=""_blank""><span class=""social foundicon-tumblr""></span></a></li>
</ul>
</article>
</section>
</footer>
<!--[if IE]>
<script>
	head.js({""jquery"":""http://www.funimation.com/assets/js/jquery-1.8.3.min.js""});
    head.js({""jqueryui"":""http://www.funimation.com/assets/js/jquery-ui-1.9.2.min.js""});
    head.js(""http://www.funimation.com/assets/js/default.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/alertify.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/bootstrap-dropdown.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/chosen.jquery.min.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/countdown.min.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/coverflow.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/filtrify.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/flexslider.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/hiddenElements.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/highlight.pack.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/jquery.carouFredSel-6.2.1-packed.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/jquery.dd.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/jquery.dotdotdot-1.5.9.min.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/jquery.fancybox.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/jquery.gridster.min.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/jQuery.Hashtable-1.1.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/jquery.lazyload.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/jquery.ui.selectmenu.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/js-url.min.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/popover.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/spin.min.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/tooltip.js"");
    head.js(""http://www.funimation.com/assets/js/libraries/zoom.js"");
</script>
<![endif]-->
<!--[if !IE]><!-->
<script type=""text/rocketscript"">
    head.js({""jquery"":""http://www.funimation.com/assets/js/jquery-1.8.3.min.js""});
    head.js({""jqueryui"":""http://www.funimation.com/assets/js/jquery-ui-1.9.2.min.js""});
    head.js(""http://www.funimation.com/assets/js/default.js"");
</script>
<!--<![endif]-->
<script type=""text/rocketscript"">    
	head.js(""http://www.funimation.com/assets/js/libraries/jPages.min.js"");
    head.js({""global"":""http://www.funimation.com/assets/js/global.js""});
    head.js(""http://w.sharethis.com/button/buttons.js"");
    head.js({""share_funi"":""http://www.funimation.com/assets/js/share_funi_style.js""});
    head.js(""http://www.funimation.com/assets/js/tiny_mce/tiny_mce_gzip.js"");
</script>
<input type=""hidden"" name=""bu"" id=""buShareFuni"" value=""http://www.funimation.com/""/>
<script type=""text/rocketscript"">
    (function () {
        var newrelic = document.createElement(""script"");
        var snewrelic = document.getElementsByTagName(""script"")[0];
        newrelic.async = true;
        newrelic.src = ""//s.btstatic.com/tag.js#site=VjnP9AP"";
        snewrelic.parentNode.insertBefore(newrelic, snewrelic);
    }());
</script>
<noscript>
<iframe src=""//s.thebrighttag.com/iframe?c=VjnP9AP"" width=""1"" height=""1"" frameborder=""0"" scrolling=""no"" marginheight=""0"" marginwidth=""0""></iframe>
</noscript>
 <script type=""text/rocketscript"">
var selectedTab = 2;
var container = null;

head.ready(function() {
        global.initToggles();
        global.initShowListToggle();
        global.initVideoTopSlider();
        global.slider();
        global.tooltips();
        global.popUp();
        //global.initFancyBox();
        global.initTabs();

        $.getScript(base_url + ""assets/js/common.js"");

        /*
        $('body').on('click','.addToMyShows',function(e){
            e.preventDefault();
            var id = $(this).attr('data-id');
            $.ajax({
                type: ""POST"",
                data: {id: id},
                url: '',
                dataType: ""html"",
                success: function(msg){
                    if(parseInt(msg) == 1)
                    {
                        $('addshows [data-id=""'+id+'""]').removeClass('addToMyShows');
                        $('addshows [data-id=""'+id+'""]').addClass('removeFromMyShows');


                        $('addshows [data-id=""'+id+'""] span').removeClass();
                        $('addshows [data-id=""'+id+'""] span').addClass('icon-minus bold');
                        $('addshows [data-id=""'+id+'""] span').attr('data-original-title','Remove from My Shows');
                    }
                }
            });
        });

        $('body').on('click','.removeFromMyShows',function(e){
            e.preventDefault();
            var id = $(this).attr('data-id');
            $.ajax({
                type: ""POST"",
                data: {id: id},
                url: '',
                dataType: ""html"",
                success: function(msg){
                    if(parseInt(msg) == 1)
                    {
                        $('addshows [data-id=""'+id+'""]').removeClass('removeFromMyShows');
                        $('addshows [data-id=""'+id+'""]').addClass('addToMyShows');

                        $('addshows [data-id=""'+id+'""] span').removeClass();
                        $('addshows [data-id=""'+id+'""] span').addClass('icon-plus bold');
                        $('addshows [data-id=""'+id+'""] span').attr('data-original-title','Add to My Shows');
                    }
                }
            });
        });


        $('body').on('click','.addToFavorites',function(e){
            e.preventDefault();
            var id = $(this).attr('data-id');
            $.ajax({
                type: ""POST"",
                data: {id: id},
                url: '',
                dataType: ""html"",
                success: function(msg){
                    if(parseInt(msg) == 1)
                    {
                        $('addfavorites [data-id=""'+id+'""]').removeClass('addToFavorites');
                        $('addfavorites [data-id=""'+id+'""]').addClass('removeFromFavorites');
                        $('addfavorites [data-id=""'+id+'""] span').addClass('red');

                        $('addfavorites [data-id=""'+id+'""] span').attr('data-original-title','Remove from Favorites');
                    }
                }
            });
        })*/

        
        <!-- End Add & remove shows from favorites -->

       // sort
        $('.sort-field').off('click').on('click', function(e){
            sortItems(false, $(e.currentTarget));
        });

        sortItems(true, null);

        $(""li[role='tab']"").click(function(e){
            if($(e.currentTarget).attr('aria-controls').indexOf('tabs-1') > 0)
                selectedTab = 2;
            else
                selectedTab = 1;        
        });


    });
/*
head.ready(""share_funi"",function() {
	addTagsImages('videos');
});*/

function sortItems(firstLoad, e){
    if (firstLoad){ 
        //var column = $("".sort-field[data-field='date']"");
        //scheduleByTypeSearch(column, 'date', 'asc', 1);
        //scheduleByTypeSearch(column, 'date', 'asc', 2);
        //scheduleBluRaySearch(null, null, null);
        loadSchedules();
    }
    else
        scheduleByTypeSearch(e, e.data('field'), e.data('order'), selectedTab);
}

function scheduleByTypeSearch(column, order_by, order_sort, type) {
    $.ajax({    
        type: ""GET"",
        data: {order_by: order_by, order_sort: order_sort, type:type},
        url: 'http://www.funimation.com/videos/schedule_streaming_subscription_search_ajax',
        dataType: ""json"",
        success: function(result){
            //$('.container').html(result['items']);
            var container = null;
            if(type == 1)
                container = '#normal-slider-with-tabs-2';        
            else
                container = '#normal-slider-with-tabs-1';                    
            $(container).html(result['items']);   

            // change sort order
            if (order_sort == ""asc"")
                $(container+"" .sort-field[data-field='""+order_by+""']"").data('order', 'desc');
            else if (order_sort == ""desc"") 
                $(container+"" .sort-field[data-field='""+order_by+""']"").data('order', 'asc');

            // sort
            $('.sort-field').off('click').on('click', function(e){
                sortItems(false, $(e.currentTarget));
            });

            global.popUp();
            global.slider();
            //global.initFancyBox();
        }
    });
}


function loadSchedules() {
	//Load schedule_streaming_subscription_search_ajax Type = 1
    $.ajax({    
        type: ""GET"",
        data: {order_by: ""date"", order_sort: ""asc"", type:1},
        url: 'http://www.funimation.com/videos/schedule_streaming_subscription_search_ajax',
        dataType: ""json"",
        success: function(result){
            var container = null;
			container = '#normal-slider-with-tabs-2';        
            $(container).html(result['items']);   

            //Load schedule_streaming_subscription_search_ajax Type = 2
            $.ajax({    
                type: ""GET"",
                data: {order_by: ""date"", order_sort: ""asc"", type:2},
                url: 'http://www.funimation.com/videos/schedule_streaming_subscription_search_ajax',
                dataType: ""json"",
                success: function(result){
                    container = '#normal-slider-with-tabs-1';        
                    $(container).html(result['items']);   

                    // sort
                    $('.sort-field').off('click').on('click', function(e){
                        sortItems(false, $(e.currentTarget));
                    });

                    global.popUp();
                    global.slider();
                    //global.initFancyBox();
                }
            });
        }
    });
}

</script>
<script type=""text/rocketscript"">if(!NREUMQ.f){NREUMQ.f=function(){NREUMQ.push([""load"",new Date().getTime()]);var e=document.createElement(""script"");e.type=""text/javascript"";e.src=((""http:""===document.location.protocol)?""http:"":""https:"")+""//""+""js-agent.newrelic.com/nr-100.js"";document.body.appendChild(e);if(NREUMQ.a)NREUMQ.a();};NREUMQ.a=window.onload;window.onload=NREUMQ.f;};NREUMQ.push([""nrfj"",""beacon-5.newrelic.com"",""2b4c127a96"",""2420982"",""YABTYRRUCEpYBkcMCVlKcFYSXAlXFjNaAQNYFh5cCFEDQQ=="",0,233,new Date().getTime(),"""","""","""","""",""""]);</script></body>
</html>";
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams.Tests
//
// AnimeRecs.UpdateStreams.Tests is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams.Tests is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.Tests.  If not, see <http://www.gnu.org/licenses/>.
//
//  If you modify AnimeRecs.UpdateStreams.Tests, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams.Tests grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.