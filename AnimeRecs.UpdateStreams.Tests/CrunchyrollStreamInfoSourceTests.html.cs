using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.UpdateStreams.Tests
{
    public partial class CrunchyrollStreamInfoSourceTests
    {
        public static string TestHtml = @"

<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""utf-8"" />
        <meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1"">
        <link rel=""shortcut icon"" href=""/favicon.ico?v=beta"" sizes=""16x16""/>
    <link rel=""apple-touch-icon"" href=""http://static.ak.crunchyroll.com/i/beta/ios_icon/cr_ios.png"" />
    <link rel=""alternate"" type=""application/rss+xml"" title=""Latest Shows on Crunchyroll"" href=""http://feeds.feedburner.com/crunchyroll/rss"" />
    <link rel=""alternate"" type=""application/rss+xml"" title=""Latest Anime News"" href=""http://feeds.feedburner.com/crunchyroll/animenews"" />
            <meta property=""og:site_name"" content=""Crunchyroll"" />
    <meta property=""og:type"" content=""tv_show"" />
    <meta name=""og:description"" content=""Browse Anime Alphabetically""/>
                <link rel=""stylesheet"" type=""text/css"" href=""http://static.ak.crunchyroll.com/css/20120820143854.1b440987cc16a76856f265005f4e5adf/crcommon/reset.css""/>
        <link rel=""stylesheet"" type=""text/css"" href=""http://static.ak.crunchyroll.com/css/20120820143854.1b440987cc16a76856f265005f4e5adf/../main_beta.css?20120820143854.1b440987cc16a76856f265005f4e5adf""/>
        <link rel=""stylesheet"" type=""text/css"" href=""http://static.ak.crunchyroll.com/css/20120820143854.1b440987cc16a76856f265005f4e5adf/../header.css?20120820143854.1b440987cc16a76856f265005f4e5adf""/>
        <link rel=""stylesheet"" type=""text/css"" href=""http://static.ak.crunchyroll.com/css/20120820143854.1b440987cc16a76856f265005f4e5adf/view/beta/videos.css""/>
    
    <title>Crunchyroll - Browse Anime Alphabetically</title>
    <script>if(typeof console==='undefined'){(function(){var noOp=function(){return;};console={log:noOp,info:noOp,warn:noOp,error:noOp,assert:noOp,dir:noOp,clear:noOp,profile:noOp,profileEnd:noOp};})();}</script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/common/static/js/jquery-1.5.1.min.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/common/static/js/jquery.tmpl.min.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/common/static/js/jquery.json-2.3.min.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/common/static/js/jquery-ui-1.8.14.custom.min.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/common/static/js/underscore-1.3.3.min.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/common/static/js/backbone-0.9.2.min.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/common/static/js/swfobject.js""></script>
        <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/jquery.jqEasyCharCounter.min.js""></script>
          <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/common_beta.js""></script>
        <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/cr.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/bb.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/jquery.caret-range-1.0.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/bubble.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/typeahead.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/ads.js""></script>
    <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/jquery.autoellipsis.min.js""></script>
        <script type=""text/javascript"" src=""http://static.ak.crunchyroll.com/js/20120820143635.4dc6ff1443f126fad6608fddbc8a1e0a/jquery/videos.js""></script>
        <!--[if lt IE 9]>
      <script src=""//html5shiv.googlecode.com/svn/trunk/html5.js""></script>
    <![endif]-->
    <script type=""text/javascript"">
      var DOMAIN = ""crunchyroll.com"";
      var AJAXROOT = ""\/ajax\/?"";
      var WEBROOT = ""http:\/\/www.crunchyroll.com"";
      var CACHED_TRANSLATIONS = [];
      var LOCALE = ""enUS"";
      var page = {};
    </script>
    <script type=""text/javascript"" src=""//apis.google.com/js/plusone.js"">
      {parsetags: 'explicit'}
    </script>
    <!--[if IE]>
    <style type=""text/css"">
      .clearfix {
        zoom: 1;     /* triggers hasLayout */
      }
      #header_searchpanel_beta #header_search_input {
        line-height: 27px;
      }
          </style>
    <![endif]-->
            <meta name=""google-site-verification"" content=""zuHV4BzoWdv29ExvUbbNKy67VvRS8WWpE1FQThEA-r0"" />
  </head>
  <body class=""main-page"">
    <div id=""template_scroller"">
            <div id=""template_container"" class=""cf template-container template-container-beta"">
        
<div id=""header_beta"" class=""container-shadow clearfix cf"">
  <div class=""left"">
    <a href=""/"" id=""logo_beta"" class=""left logo-enUS""></a>
    <ul id=""header_menubar_beta"" class=""left clearfix cf"">
      <!-- 
      <li class=""menubar-item-beta"">
        <a href=""/"" token=""topbar"">Home</a>
      </li> -->
      <li class=""menubar-item-beta menubar-item-beta-selected"">
                <a href=""/videos/anime"" token=""topbar"">Shows</a>
              </li>
            <li class=""menubar-item-beta"">
        <a href=""/news"" token=""topbar"" >News</a>
      </li>
            <li class=""menubar-item-beta"">
        <a href=""/forum"" token=""topbar"">Forums</a>
      </li>
      <li class=""menubar-item-beta"">
        <a href=""/deals"" token=""topbar"">Deals</a>
      </li>
            <li class=""menubar-item-beta"">
        <a href=""/freetrial/anime/?from=topbar"" token=""topbar"">Premium</a>
        <span class=""superscript-free"">Free</span>
      </li>
          </ul>
  </div><!-- left -->
  
  <div class=""right clearfix cf"">
    <ul id=""header_userpanel_beta"" class=""clearfix left"">
              <li class=""login left""><a href=""/login?next=%2Fvideos%2Fanime%2Falpha%3Fgroup%3Dall"" rel=""nofollow"" token=""login_top"">Log In</a></li>
          </ul>
  
    <ul id=""header_searchpanel_beta"" class=""clearfix cf left"">
      <li class=""search left"">
        <div id=""header_search_beta"">
          <form method=""get"" action=""/search"" id=""main_searchbox"" class=""clearfix cf"">
            <input type=""text"" name=""q"" placeholder=""Anime series, drama, etc"" value="""" id=""header_search_input"" class=""placeholder header-searchbox left"" autocomplete=""off""/>
            <a href=""javascript:void(0)"" id=""header_search_submit_beta"" class=""left header-searchbox-submit"" title=""Search""></a>
          </form>
          <script type=""text/javascript"">

            $(""#header_search_input"").val($(""#header_search_input"").attr(""placeholder""));
            $(""#header_search_input"").focus(function(){
              focus_text($(""#header_search_input""), $(""#header_search_input"").attr(""placeholder""));
              ComboSearch.preload();
              ComboSearch.search(""header_search_input"");
            
              $('#header_search_submit_beta').addClass('searchbox-focus');
            });
            
            $(""#header_search_input"").blur(function() {
              blur_text($(""#header_search_input""), $(""#header_search_input"").attr(""placeholder""));
              $('#header_search_submit_beta').removeClass('searchbox-focus');
            });
            
            $(""#header_search_input"").keyup(function(e) { 
              ComboSearch.search(""header_search_input""); 
              if(e.keyCode == 27){
                ComboSearch.close();
                $(this).val('');
              }
            });
            $(""#header_search_input"").keydown(ComboSearch.handleKeyPress);
            $(document).click(ComboSearch.handleDocumentClick);
            
            $(""#header_search_submit_beta"").click(function() {
              if($(""#header_search_input"").val()==$(""#header_search_input"").attr(""placeholder"")) {
                return false;
              }
              $(""#header_search_input"").closest(""form"").submit();
            });
            
          </script>
          <div id=""header_search_autocomplete"" style=""display: none;""> </div>
        </div>
      </li><!-- search -->
    </ul><!-- search_panel -->
  </div>
</div><!-- header -->
<script type=""text/javascript"">
  //// tool tip  
  // hide or show
  var thisLi;
  $(""#header_userpanel_beta li"").mouseover(function(){
    thisLi = $(this);
    showHeaderTooltip(thisLi, thisLi.find("".tooltip""));
    thisLi.find(""a"").mousedown(function(){
      hideTooltip(thisLi, thisLi.find("".tooltip""));
    });
  });
  $(""#header_userpanel_beta li"").mouseout(function(){
    hideTooltip(thisLi, thisLi.find("".tooltip""));
  });
  function hideTooltip(parent, tooltip){
    clearTimeout(parent.data('timeout'));
    tooltip.stop(true, true).animate({opacity:0}, 200, function(){
      $(this).css({'visibility' : 'hidden'});
    });
  }
  function showHeaderTooltip(parent, tooltip){
    var tooltipDesc = tooltip.find("".tooltip-desc"");
    alignTooltip(tooltipDesc);
    parent.data('timeout', setTimeout(function(){
      tooltip.css({'visibility' : 'visible', 'opacity' : 0}).animate({opacity:1}, 200);
    }, 450));
  }
  
  // align tool tip
  function alignTooltip(tooltipDesc){
    var tooltipWidth;
    tooltipWidth = tooltipDesc.outerWidth();
    tooltipMargin = -tooltipWidth / 2;
    tooltipDesc.parent().css({'left' : '50%', 'margin-left' : tooltipMargin, 'width' : tooltipWidth});
  }
  
  // set width for arrows (ie9 fix)
  function setWidth(divHasWidth, divNoWidth){
    var divWithWidth;
    divWithWidth = divHasWidth.outerWidth();
    divNoWidth.css({'width' : divWithWidth});
  }
  
  //// end of tool tip
  
  //// user drop down
  var userMenu = $(""#header_userpanel_beta .user-icon"");
  userMenu.click(function(event){
    dropdownToggle($(this));
    userMenu.toggleClass(""open"");
    event.stopPropagation();
  });
  
  var userDropdown = $("".dropdown"");
  function dropdownToggle(sibling){
    sibling.next(userDropdown).toggle();
    setWidth(userDropdown.find('.dropdown-menu'), userDropdown.find('.tooltip-top'));
  }
  $(userDropdown).click(function(event){
    event.stopPropagation();
  });

  $('html').click(function(){
    $("".dropdown"").hide();
    userMenu.removeClass(""open"");
  });
  //// end of drop down
</script>
                      <div id=""template_skin_leaderboard"" class=""clearfix container-shadow""><script type=""text/javascript"" src=""http://adserver.adtechus.com/addyn/3.0/5290/1285662/0/999/ADTECH;loc=100;target=_blank;grp=68297359;misc=68297359""></script>
<noscript><a href=""http://adserver.adtechus.com/adlink/3.0/5290/1285662/0/16/ADTECH;loc=300;grp=68297359"" target=""_blank""><img src=""http://adserver.adtechus.com/adserv/3.0/5290/1285662/0/16/ADTECH;loc=300;grp=68297359"" border=""0"" width=""1"" height=""1""/></a></noscript></div>
                <div id=""template_body"" class=""cf"">
                    <div id=""message_box"" style=""display: none;"">
          </div>
          <script type=""text/javascript"">
            Page.messaging_box_controller = new MessageBox({""render_to"": ""#message_box""});
            Page.messaging_box_controller.addItems([]);
          </script>

          <div style=""height:0;visibility:hidden;"" class=""clearfix""></div>
          
      
<div id=""source_browse"">
  <div id=""tabs"" class=""medium-margin-bottom container-shadow"">
    <div class=""main-tabs cf"">
      <a href=""/videos/anime"" token=""shows-anime"" class=""left selected"">Anime</a>
            <a href=""/videos/drama"" token=""shows-drama"" class=""left "">Drama</a>
          </div>
    <div class=""sub-tabs cf"">
      <div class=""sub-tabs-menu cf"">
        <a href=""/videos/anime/popular"" token=""shows-popular"" class=""left block-link "">Popular</a>
        <a href=""/videos/anime/simulcasts"" token=""shows-simulcasts"" class=""left block-link "">Simulcasts</a>
        <a href=""/videos/anime/updated"" token=""shows-updated"" class=""left block-link "">Updated</a>
        <a href=""/videos/anime/alpha"" token=""shows-alpha"" class=""left block-link selected"">Alphabetical</a>
        <a href=""/videos/anime/genres"" token=""shows-genres"" id=""genres_link"" class=""left block-link "">Genres</a>
        <a href=""/videos/anime/seasons"" token=""shows-seasons"" id=""seasons_link"" class=""left block-link "">Seasons</a>
      </div>
      <div class=""genre-selectors selectors"">
        <form>
          <ul class=""cf"">
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""action"" name=""action"" value=""action"" type=""checkbox""  title=""Action""/>
              <label for=""action"" class=""text-link"">Action</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""adventure"" name=""adventure"" value=""adventure"" type=""checkbox""  title=""Adventure""/>
              <label for=""adventure"" class=""text-link"">Adventure</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""comedy"" name=""comedy"" value=""comedy"" type=""checkbox""  title=""Comedy""/>
              <label for=""comedy"" class=""text-link"">Comedy</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""drama"" name=""drama"" value=""drama"" type=""checkbox""  title=""Drama""/>
              <label for=""drama"" class=""text-link"">Drama</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""ecchi"" name=""ecchi"" value=""ecchi"" type=""checkbox""  title=""Ecchi""/>
              <label for=""ecchi"" class=""text-link"">Ecchi</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""fantasy"" name=""fantasy"" value=""fantasy"" type=""checkbox""  title=""Fantasy""/>
              <label for=""fantasy"" class=""text-link"">Fantasy</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""historical"" name=""historical"" value=""historical"" type=""checkbox""  title=""Historical""/>
              <label for=""historical"" class=""text-link"">Historical</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""mecha"" name=""mecha"" value=""mecha"" type=""checkbox""  title=""Mecha""/>
              <label for=""mecha"" class=""text-link"">Mecha</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""romance"" name=""romance"" value=""romance"" type=""checkbox""  title=""Romance""/>
              <label for=""romance"" class=""text-link"">Romance</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""science fiction"" name=""science fiction"" value=""science fiction"" type=""checkbox""  title=""Science Fiction""/>
              <label for=""science fiction"" class=""text-link"">Science Fiction</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""seinen"" name=""seinen"" value=""seinen"" type=""checkbox""  title=""Seinen/Mature""/>
              <label for=""seinen"" class=""text-link"">Seinen/Mature</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""shoujo"" name=""shoujo"" value=""shoujo"" type=""checkbox""  title=""Shoujo""/>
              <label for=""shoujo"" class=""text-link"">Shoujo</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""shounen"" name=""shounen"" value=""shounen"" type=""checkbox""  title=""Shounen""/>
              <label for=""shounen"" class=""text-link"">Shounen</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""slice of life"" name=""slice of life"" value=""slice of life"" type=""checkbox""  title=""Slice of Life""/>
              <label for=""slice of life"" class=""text-link"">Slice of Life</label>
            </li>
                      <li class=""left medium-margin-right small-margin-bottom ellipsis"">
              <input id=""sports"" name=""sports"" value=""sports"" type=""checkbox""  title=""Sports""/>
              <label for=""sports"" class=""text-link"">Sports</label>
            </li>
                    </ul>
        </form>
      </div>
      <div class=""season-selectors cf selectors"">
        <ul class=""cf"">
                                    <li class=""left""><a href=""#/videos/anime/seasons/summer-2012"" class=""text-link large-margin-right season"" id=""season:summer_2012"" title=""Summer 2012"">Summer 2012</a></li>
                                                <li class=""left""><a href=""#/videos/anime/seasons/spring-2012"" class=""text-link large-margin-right season"" id=""season:spring_2012"" title=""Spring 2012"">Spring 2012</a></li>
                                                <li class=""left""><a href=""#/videos/anime/seasons/winter-2012"" class=""text-link large-margin-right season"" id=""season:winter_2012"" title=""Winter 2012"">Winter 2012</a></li>
                                                <li class=""left""><a href=""#/videos/anime/seasons/fall-2011"" class=""text-link large-margin-right season"" id=""season:fall_2011"" title=""Fall 2011"">Fall 2011</a></li>
                                                <li class=""left""><a href=""#/videos/anime/seasons/summer-2011"" class=""text-link large-margin-right season"" id=""season:summer_2011"" title=""Summer 2011"">Summer 2011</a></li>
                                                <li class=""left""><a href=""#/videos/anime/seasons/spring-2011"" class=""text-link large-margin-right season"" id=""season:spring_2011"" title=""Spring 2011"">Spring 2011</a></li>
                                                <li class=""left""><a href=""#/videos/anime/seasons/winter-2011"" class=""text-link large-margin-right season"" id=""season:winter_2011"" title=""Winter 2011"">Winter 2011</a></li>
                                                <li class=""left""><a href=""#/videos/anime/seasons/fall-2010"" class=""text-link large-margin-right season"" id=""season:fall_2010"" title=""Fall 2010"">Fall 2010</a></li>
                                                                <li class=""right""><a class=""text-link more"">More</a></li>
              <div class=""dropdown"">
                <div class=""tooltip-top""></div>
                <div class=""dropdown-menu"">
                  <ul class=""list-block"">
                                                                                  <li><a href=""#/videos/anime/seasons/spring-2010"" class=""season"" id=""season:spring_2010"" title=""Spring 2010"">Spring 2010</a></li>
                                                                                  <li><a href=""#/videos/anime/seasons/winter-2010"" class=""season"" id=""season:winter_2010"" title=""Winter 2010"">Winter 2010</a></li>
                                                                                  <li><a href=""#/videos/anime/seasons/fall-2009"" class=""season"" id=""season:fall_2009"" title=""Fall 2009"">Fall 2009</a></li>
                                                                                  <li><a href=""#/videos/anime/seasons/summer-2009"" class=""season"" id=""season:summer_2009"" title=""Summer 2009"">Summer 2009</a></li>
                                                                                  <li><a href=""#/videos/anime/seasons/spring-2009"" class=""season"" id=""season:spring_2009"" title=""Spring 2009"">Spring 2009</a></li>
                                                                                  <li><a href=""#/videos/anime/seasons/winter-2009"" class=""season"" id=""season:winter_2009"" title=""Winter 2009"">Winter 2009</a></li>
                                                                  </ul>
                </div>
              </div>
                </ul>            
      </div>
    </div>
  </div>

  
  <div id=""container"" class=""cf"">
    <div id=""main_content"" class=""left"">
              <div id=""content-menu-top"" class=""medium-margin-bottom"">
          <div class=""content-menu cf"">
                          <a href=""/videos/anime/alpha?group=a""
                 class=""left text-link strong "">A</a>
                          <a href=""/videos/anime/alpha?group=b""
                 class=""left text-link strong "">B</a>
                          <a href=""/videos/anime/alpha?group=c""
                 class=""left text-link strong "">C</a>
                          <a href=""/videos/anime/alpha?group=d""
                 class=""left text-link strong "">D</a>
                          <a href=""/videos/anime/alpha?group=e""
                 class=""left text-link strong "">E</a>
                          <a href=""/videos/anime/alpha?group=f""
                 class=""left text-link strong "">F</a>
                          <a href=""/videos/anime/alpha?group=g""
                 class=""left text-link strong "">G</a>
                          <a href=""/videos/anime/alpha?group=h""
                 class=""left text-link strong "">H</a>
                          <a href=""/videos/anime/alpha?group=i""
                 class=""left text-link strong "">I</a>
                          <a href=""/videos/anime/alpha?group=j""
                 class=""left text-link strong "">J</a>
                          <a href=""/videos/anime/alpha?group=k""
                 class=""left text-link strong "">K</a>
                          <a href=""/videos/anime/alpha?group=l""
                 class=""left text-link strong "">L</a>
                          <a href=""/videos/anime/alpha?group=m""
                 class=""left text-link strong "">M</a>
                          <a href=""/videos/anime/alpha?group=n""
                 class=""left text-link strong "">N</a>
                          <a href=""/videos/anime/alpha?group=o""
                 class=""left text-link strong "">O</a>
                          <a href=""/videos/anime/alpha?group=p""
                 class=""left text-link strong "">P</a>
                          <a href=""/videos/anime/alpha?group=q""
                 class=""left text-link strong "">Q</a>
                          <a href=""/videos/anime/alpha?group=r""
                 class=""left text-link strong "">R</a>
                          <a href=""/videos/anime/alpha?group=s""
                 class=""left text-link strong "">S</a>
                          <a href=""/videos/anime/alpha?group=t""
                 class=""left text-link strong "">T</a>
                          <a href=""/videos/anime/alpha?group=u""
                 class=""left text-link strong "">U</a>
                          <a href=""/videos/anime/alpha?group=v""
                 class=""left text-link strong "">V</a>
                          <a href=""/videos/anime/alpha?group=w""
                 class=""left text-link strong "">W</a>
                          <a href=""/videos/anime/alpha?group=x""
                 class=""left text-link strong "">X</a>
                          <a href=""/videos/anime/alpha?group=y""
                 class=""left text-link strong "">Y</a>
                          <a href=""/videos/anime/alpha?group=z""
                 class=""left text-link strong "">Z</a>
                        <a href=""/videos/anime/alpha?group=numeric"" class=""left text-link strong "">#</a>
            <a href=""/videos/anime/alpha?group=all"" class=""right text-link strong selected"">View All</a>
          </div>
        </div>
                  <div class=""videos-column-container cf"">
                      
                    <div class=""videos-column left"">
                        <h3>0</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_170134"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""170134"">
    <a title=""07 Ghost"" token=""shows-portraits"" itemprop=""url"" href=""/07-ghost"" class=""text-link ellipsis"">
      07 Ghost    </a>
  </li>
                            </ul>
                        <h3>1</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_192242"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""192242"">
    <a title=""11eyes"" token=""shows-portraits"" itemprop=""url"" href=""/11eyes"" class=""text-link ellipsis"">
      11eyes    </a>
  </li>
                            </ul>
                        <h3>A</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_229030"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229030"">
    <a title=""A Bridge to the Starry Skies - Hoshizora e Kakaru Hashi"" token=""shows-portraits"" itemprop=""url"" href=""/a-bridge-to-the-starry-skies-hoshizora-e-kakaru-hashi"" class=""text-link ellipsis"">
      A Bridge to the Starry Skies - Hoshizora e Kakaru Hashi    </a>
  </li>
                                  <li id=""media_group_234113"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234113"">
    <a title=""A Dark Rabbit has Seven Lives"" token=""shows-portraits"" itemprop=""url"" href=""/a-dark-rabbit-has-seven-lives"" class=""text-link ellipsis"">
      A Dark Rabbit has Seven Lives    </a>
  </li>
                                  <li id=""media_group_156063"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""156063"">
    <a title=""Abunai Sisters"" token=""shows-portraits"" itemprop=""url"" href=""/abunai-sisters"" class=""text-link ellipsis"">
      Abunai Sisters    </a>
  </li>
                                  <li id=""media_group_153359"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""153359"">
    <a title=""Adventures in Voice Acting"" token=""shows-portraits"" itemprop=""url"" href=""/adventures-in-voice-acting"" class=""text-link ellipsis"">
      Adventures in Voice Acting    </a>
  </li>
                                  <li id=""media_group_47180"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47180"">
    <a title=""Air Master"" token=""shows-portraits"" itemprop=""url"" href=""/air-master"" class=""text-link ellipsis"">
      Air Master    </a>
  </li>
                                  <li id=""media_group_245914"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""245914"">
    <a title=""Angel Beats"" token=""shows-portraits"" itemprop=""url"" href=""/angel-beats"" class=""text-link ellipsis"">
      Angel Beats    </a>
  </li>
                                  <li id=""media_group_230882"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""230882"">
    <a title=""AniView"" token=""shows-portraits"" itemprop=""url"" href=""/aniview"" class=""text-link ellipsis"">
      AniView    </a>
  </li>
                                  <li id=""media_group_240790"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240790"">
    <a title=""Ano Natsu de Matteru"" token=""shows-portraits"" itemprop=""url"" href=""/ano-natsu-de-matteru"" class=""text-link ellipsis"">
      Ano Natsu de Matteru    </a>
  </li>
                                  <li id=""media_group_247214"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""247214"">
    <a title=""Anohana: The Flower We Saw That Day"" token=""shows-portraits"" itemprop=""url"" href=""/anohana-the-flower-we-saw-that-day"" class=""text-link ellipsis"">
      Anohana: The Flower We Saw That Day    </a>
  </li>
                                  <li id=""media_group_240622"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240622"">
    <a title=""Another"" token=""shows-portraits"" itemprop=""url"" href=""/another"" class=""text-link ellipsis"">
      Another    </a>
  </li>
                                  <li id=""media_group_219471"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""219471"">
    <a title=""Artichoke and Peachies Show"" token=""shows-portraits"" itemprop=""url"" href=""/artichoke-and-peachies-show"" class=""text-link ellipsis"">
      Artichoke and Peachies Show    </a>
  </li>
                                  <li id=""media_group_213938"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""213938"">
    <a title=""Asobi ni Ikuyo: Bombshells from the Sky"" token=""shows-portraits"" itemprop=""url"" href=""/asobi-ni-ikuyo-bombshells-from-the-sky"" class=""text-link ellipsis"">
      Asobi ni Ikuyo: Bombshells from the Sky    </a>
  </li>
                                  <li id=""media_group_229044"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229044"">
    <a title=""Astarotte's Toy"" token=""shows-portraits"" itemprop=""url"" href=""/astarottes-toy"" class=""text-link ellipsis"">
      Astarotte's Toy    </a>
  </li>
                                  <li id=""media_group_119408"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""119408"">
    <a title=""Asura Cryin'"" token=""shows-portraits"" itemprop=""url"" href=""/asura-cryin"" class=""text-link ellipsis"">
      Asura Cryin'    </a>
  </li>
                                  <li id=""media_group_241656"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""241656"">
    <a title=""AX Live"" token=""shows-portraits"" itemprop=""url"" href=""/ax-live"" class=""text-link ellipsis"">
      AX Live    </a>
  </li>
                            </ul>
                        <h3>B</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_229670"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229670"">
    <a title=""Battle Girls - Time Paradox"" token=""shows-portraits"" itemprop=""url"" href=""/battle-girls-time-paradox"" class=""text-link ellipsis"">
      Battle Girls - Time Paradox    </a>
  </li>
                                  <li id=""media_group_225542"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""225542"">
    <a title=""Beelzebub"" token=""shows-portraits"" itemprop=""url"" href=""/beelzebub"" class=""text-link ellipsis"">
      Beelzebub    </a>
  </li>
                                  <li id=""media_group_52948"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""52948"">
    <a title=""Black Jack"" token=""shows-portraits"" itemprop=""url"" href=""/black-jack"" class=""text-link ellipsis"">
      Black Jack    </a>
  </li>
                                  <li id=""media_group_197925"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""197925"">
    <a title=""Black Jack Motion Magazine"" token=""shows-portraits"" itemprop=""url"" href=""/black-jack-motion-magazine"" class=""text-link ellipsis"">
      Black Jack Motion Magazine    </a>
  </li>
                                  <li id=""media_group_62210"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""62210"">
    <a title=""BLASSREITER"" token=""shows-portraits"" itemprop=""url"" href=""/blassreiter"" class=""text-link ellipsis"">
      BLASSREITER    </a>
  </li>
                                  <li id=""media_group_42854"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""42854"">
    <a title=""Bleach"" token=""shows-portraits"" itemprop=""url"" href=""/bleach"" class=""text-link ellipsis"">
      Bleach    </a>
  </li>
                                  <li id=""media_group_247566"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""247566"">
    <a title=""Blue Drop"" token=""shows-portraits"" itemprop=""url"" href=""/blue-drop"" class=""text-link ellipsis"">
      Blue Drop    </a>
  </li>
                                  <li id=""media_group_226293"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""226293"">
    <a title=""Blue Exorcist"" token=""shows-portraits"" itemprop=""url"" href=""/blue-exorcist"" class=""text-link ellipsis"">
      Blue Exorcist    </a>
  </li>
                                  <li id=""media_group_240614"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240614"">
    <a title=""Bodacious Space Pirates"" token=""shows-portraits"" itemprop=""url"" href=""/bodacious-space-pirates"" class=""text-link ellipsis"">
      Bodacious Space Pirates    </a>
  </li>
                                  <li id=""media_group_240618"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240618"">
    <a title=""Brave 10"" token=""shows-portraits"" itemprop=""url"" href=""/brave-10"" class=""text-link ellipsis"">
      Brave 10    </a>
  </li>
                                  <li id=""media_group_230548"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""230548"">
    <a title=""Break Ups"" token=""shows-portraits"" itemprop=""url"" href=""/break-ups"" class=""text-link ellipsis"">
      Break Ups    </a>
  </li>
                            </ul>
                        <h3>C</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_246842"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246842"">
    <a title=""Campione!"" token=""shows-portraits"" itemprop=""url"" href=""/campione"" class=""text-link ellipsis"">
      Campione!    </a>
  </li>
                                  <li id=""media_group_221953"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""221953"">
    <a title=""Canvas 2: Niji Iro no Sketch"" token=""shows-portraits"" itemprop=""url"" href=""/canvas-2-niji-iro-no-sketch"" class=""text-link ellipsis"">
      Canvas 2: Niji Iro no Sketch    </a>
  </li>
                                  <li id=""media_group_49950"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""49950"">
    <a title=""Captain Harlock"" token=""shows-portraits"" itemprop=""url"" href=""/captain-harlock"" class=""text-link ellipsis"">
      Captain Harlock    </a>
  </li>
                                  <li id=""media_group_224833"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""224833"">
    <a title=""Cardfight Vanguard (Season 1)"" token=""shows-portraits"" itemprop=""url"" href=""/cardfight-vanguard-season-1"" class=""text-link ellipsis"">
      Cardfight Vanguard (Season 1)    </a>
  </li>
                                  <li id=""media_group_248390"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""248390"">
    <a title=""Cardfight!! Vanguard Asia Circuit (Season 2)"" token=""shows-portraits"" itemprop=""url"" href=""/cardfight-vanguard-asia-circuit-season-2"" class=""text-link ellipsis"">
      Cardfight!! Vanguard Asia Circuit (Season 2)    </a>
  </li>
                                  <li id=""media_group_234147"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234147"">
    <a title=""Cat God"" token=""shows-portraits"" itemprop=""url"" href=""/cat-god"" class=""text-link ellipsis"">
      Cat God    </a>
  </li>
                                  <li id=""media_group_100430"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""100430"">
    <a title=""Catblue Dynamite"" token=""shows-portraits"" itemprop=""url"" href=""/catblue-dynamite"" class=""text-link ellipsis"">
      Catblue Dynamite    </a>
  </li>
                                  <li id=""media_group_89607"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""89607"">
    <a title=""Chance Pop Session"" token=""shows-portraits"" itemprop=""url"" href=""/chance-pop-session"" class=""text-link ellipsis"">
      Chance Pop Session    </a>
  </li>
                                  <li id=""media_group_179866"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""179866"">
    <a title=""Charger Girl Ju-den Chan"" token=""shows-portraits"" itemprop=""url"" href=""/charger-girl-ju-den-chan"" class=""text-link ellipsis"">
      Charger Girl Ju-den Chan    </a>
  </li>
                                  <li id=""media_group_62162"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""62162"">
    <a title=""Chi's Sweet Home - Chi's New Address"" token=""shows-portraits"" itemprop=""url"" href=""/chis-sweet-home-chis-new-address"" class=""text-link ellipsis"">
      Chi's Sweet Home - Chi's New Address    </a>
  </li>
                                  <li id=""media_group_237918"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""237918"">
    <a title=""Chihayafuru"" token=""shows-portraits"" itemprop=""url"" href=""/chihayafuru"" class=""text-link ellipsis"">
      Chihayafuru    </a>
  </li>
                                  <li id=""media_group_246432"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246432"">
    <a title=""Chitose Get You!"" token=""shows-portraits"" itemprop=""url"" href=""/chitose-get-you"" class=""text-link ellipsis"">
      Chitose Get You!    </a>
  </li>
                                  <li id=""media_group_199616"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""199616"">
    <a title=""Chu-Bra"" token=""shows-portraits"" itemprop=""url"" href=""/chu-bra"" class=""text-link ellipsis"">
      Chu-Bra    </a>
  </li>
                                  <li id=""media_group_199629"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""199629"">
    <a title=""Cobra the Animation"" token=""shows-portraits"" itemprop=""url"" href=""/cobra-the-animation"" class=""text-link ellipsis"">
      Cobra the Animation    </a>
  </li>
                                  <li id=""media_group_161434"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""161434"">
    <a title=""CODE GEASS Lelouch of the Rebellion"" token=""shows-portraits"" itemprop=""url"" href=""/code-geass-lelouch-of-the-rebellion"" class=""text-link ellipsis"">
      CODE GEASS Lelouch of the Rebellion    </a>
  </li>
                                  <li id=""media_group_48046"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""48046"">
    <a title=""Cosplay Complex OVA"" token=""shows-portraits"" itemprop=""url"" href=""/cosplay-complex-ova"" class=""text-link ellipsis"">
      Cosplay Complex OVA    </a>
  </li>
                                  <li id=""media_group_230428"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""230428"">
    <a title=""Culture Japan"" token=""shows-portraits"" itemprop=""url"" href=""/culture-japan"" class=""text-link ellipsis"">
      Culture Japan    </a>
  </li>
                            </ul>
                        <h3>D</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_178193"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""178193"">
    <a title=""Dark Side Cat"" token=""shows-portraits"" itemprop=""url"" href=""/dark-side-cat"" class=""text-link ellipsis"">
      Dark Side Cat    </a>
  </li>
                                  <li id=""media_group_48004"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""48004"">
    <a title=""Dear Boys"" token=""shows-portraits"" itemprop=""url"" href=""/dear-boys"" class=""text-link ellipsis"">
      Dear Boys    </a>
  </li>
                                  <li id=""media_group_227658"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""227658"">
    <a title=""DEMIAN"" token=""shows-portraits"" itemprop=""url"" href=""/demian"" class=""text-link ellipsis"">
      DEMIAN    </a>
  </li>
                                  <li id=""media_group_206449"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""206449"">
    <a title=""Demon King Daimao"" token=""shows-portraits"" itemprop=""url"" href=""/demon-king-daimao"" class=""text-link ellipsis"">
      Demon King Daimao    </a>
  </li>
                                  <li id=""media_group_222052"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""222052"">
    <a title=""Demonbane"" token=""shows-portraits"" itemprop=""url"" href=""/demonbane"" class=""text-link ellipsis"">
      Demonbane    </a>
  </li>
                                  <li id=""media_group_48048"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""48048"">
    <a title=""Digimon Adventure 02"" token=""shows-portraits"" itemprop=""url"" href=""/digimon-adventure-02"" class=""text-link ellipsis"">
      Digimon Adventure 02    </a>
  </li>
                                  <li id=""media_group_230332"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""230332"">
    <a title=""Digimon Tamers"" token=""shows-portraits"" itemprop=""url"" href=""/digimon-tamers"" class=""text-link ellipsis"">
      Digimon Tamers    </a>
  </li>
                                  <li id=""media_group_238724"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""238724"">
    <a title=""Digimon Xros Wars - The Young Hunters Who Leapt Through Time"" token=""shows-portraits"" itemprop=""url"" href=""/digimon-xros-wars-the-young-hunters-who-leapt-through-time"" class=""text-link ellipsis"">
      Digimon Xros Wars - The Young Hunters Who Leapt Through Time    </a>
  </li>
                                  <li id=""media_group_247112"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""247112"">
    <a title=""Dog Days"" token=""shows-portraits"" itemprop=""url"" href=""/dog-days"" class=""text-link ellipsis"">
      Dog Days    </a>
  </li>
                                  <li id=""media_group_197931"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""197931"">
    <a title=""Dororo Motion Magazine"" token=""shows-portraits"" itemprop=""url"" href=""/dororo-motion-magazine"" class=""text-link ellipsis"">
      Dororo Motion Magazine    </a>
  </li>
                                  <li id=""media_group_224841"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""224841"">
    <a title=""Dragon Crisis"" token=""shows-portraits"" itemprop=""url"" href=""/dragon-crisis"" class=""text-link ellipsis"">
      Dragon Crisis    </a>
  </li>
                                  <li id=""media_group_199620"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""199620"">
    <a title=""Durarara"" token=""shows-portraits"" itemprop=""url"" href=""/durarara"" class=""text-link ellipsis"">
      Durarara    </a>
  </li>
                                  <li id=""media_group_244418"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244418"">
    <a title=""Dusk Maiden of Amnesia"" token=""shows-portraits"" itemprop=""url"" href=""/dusk-maiden-of-amnesia"" class=""text-link ellipsis"">
      Dusk Maiden of Amnesia    </a>
  </li>
                            </ul>
                        <h3>E</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_80532"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""80532"">
    <a title=""Egg Man"" token=""shows-portraits"" itemprop=""url"" href=""/egg-man"" class=""text-link ellipsis"">
      Egg Man    </a>
  </li>
                                  <li id=""media_group_119444"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""119444"">
    <a title=""Erin"" token=""shows-portraits"" itemprop=""url"" href=""/erin"" class=""text-link ellipsis"">
      Erin    </a>
  </li>
                                  <li id=""media_group_47018"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47018"">
    <a title=""Eureka Seven"" token=""shows-portraits"" itemprop=""url"" href=""/eureka-seven"" class=""text-link ellipsis"">
      Eureka Seven    </a>
  </li>
                                  <li id=""media_group_46844"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""46844"">
    <a title=""Eyeshield 21"" token=""shows-portraits"" itemprop=""url"" href=""/eyeshield-21"" class=""text-link ellipsis"">
      Eyeshield 21    </a>
  </li>
                            </ul>
                        <h3>F</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_46806"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""46806"">
    <a title=""Fairy Musketeers"" token=""shows-portraits"" itemprop=""url"" href=""/fairy-musketeers"" class=""text-link ellipsis"">
      Fairy Musketeers    </a>
  </li>
                                  <li id=""media_group_185954"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""185954"">
    <a title=""Fairy Tail"" token=""shows-portraits"" itemprop=""url"" href=""/fairy-tail"" class=""text-link ellipsis"">
      Fairy Tail    </a>
  </li>
                                  <li id=""media_group_241208"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""241208"">
    <a title=""Familiar of Zero F"" token=""shows-portraits"" itemprop=""url"" href=""/familiar-of-zero-f"" class=""text-link ellipsis"">
      Familiar of Zero F    </a>
  </li>
                                  <li id=""media_group_238014"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""238014"">
    <a title=""Fate Zero"" token=""shows-portraits"" itemprop=""url"" href=""/fate-zero"" class=""text-link ellipsis"">
      Fate Zero    </a>
  </li>
                                  <li id=""media_group_48052"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""48052"">
    <a title=""Fist of the North Star"" token=""shows-portraits"" itemprop=""url"" href=""/fist-of-the-north-star"" class=""text-link ellipsis"">
      Fist of the North Star    </a>
  </li>
                                  <li id=""media_group_243998"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243998"">
    <a title=""Folktales from Japan"" token=""shows-portraits"" itemprop=""url"" href=""/folktales-from-japan"" class=""text-link ellipsis"">
      Folktales from Japan    </a>
  </li>
                                  <li id=""media_group_219977"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""219977"">
    <a title=""Fortune Arterial"" token=""shows-portraits"" itemprop=""url"" href=""/fortune-arterial"" class=""text-link ellipsis"">
      Fortune Arterial    </a>
  </li>
                            </ul>
                        <h3>G</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_228632"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""228632"">
    <a title=""GA Geijutsuka Art Design Class"" token=""shows-portraits"" itemprop=""url"" href=""/ga-geijutsuka-art-design-class"" class=""text-link ellipsis"">
      GA Geijutsuka Art Design Class    </a>
  </li>
                                  <li id=""media_group_230334"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""230334"">
    <a title=""Gaiking: Legend of Daiku-Maryu"" token=""shows-portraits"" itemprop=""url"" href=""/gaiking-legend-of-daiku-maryu"" class=""text-link ellipsis"">
      Gaiking: Legend of Daiku-Maryu    </a>
  </li>
                                  <li id=""media_group_46828"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""46828"">
    <a title=""Galaxy Express 999"" token=""shows-portraits"" itemprop=""url"" href=""/galaxy-express-999"" class=""text-link ellipsis"">
      Galaxy Express 999    </a>
  </li>
                                  <li id=""media_group_179116"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""179116"">
    <a title=""Galaxy High School"" token=""shows-portraits"" itemprop=""url"" href=""/galaxy-high-school"" class=""text-link ellipsis"">
      Galaxy High School    </a>
  </li>
                                  <li id=""media_group_47220"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47220"">
    <a title=""Gankutsuou"" token=""shows-portraits"" itemprop=""url"" href=""/gankutsuou"" class=""text-link ellipsis"">
      Gankutsuou    </a>
  </li>
                                  <li id=""media_group_239574"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""239574"">
    <a title=""gdgd Fairies"" token=""shows-portraits"" itemprop=""url"" href=""/gdgd-fairies"" class=""text-link ellipsis"">
      gdgd Fairies    </a>
  </li>
                                  <li id=""media_group_209908"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""209908"">
    <a title=""Giant Killing"" token=""shows-portraits"" itemprop=""url"" href=""/giant-killing"" class=""text-link ellipsis"">
      Giant Killing    </a>
  </li>
                                  <li id=""media_group_47620"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47620"">
    <a title=""Gintama"" token=""shows-portraits"" itemprop=""url"" href=""/gintama"" class=""text-link ellipsis"">
      Gintama    </a>
  </li>
                                  <li id=""media_group_46672"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""46672"">
    <a title=""Girl's High"" token=""shows-portraits"" itemprop=""url"" href=""/girls-high"" class=""text-link ellipsis"">
      Girl's High    </a>
  </li>
                                  <li id=""media_group_48766"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""48766"">
    <a title=""Glass Mask"" token=""shows-portraits"" itemprop=""url"" href=""/glass-mask"" class=""text-link ellipsis"">
      Glass Mask    </a>
  </li>
                                  <li id=""media_group_222428"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""222428"">
    <a title=""Go Lion"" token=""shows-portraits"" itemprop=""url"" href=""/go-lion"" class=""text-link ellipsis"">
      Go Lion    </a>
  </li>
                                  <li id=""media_group_117404"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""117404"">
    <a title=""Good Luck! Ninomiya-kun"" token=""shows-portraits"" itemprop=""url"" href=""/good-luck-ninomiya-kun"" class=""text-link ellipsis"">
      Good Luck! Ninomiya-kun    </a>
  </li>
                                  <li id=""media_group_225573"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""225573"">
    <a title=""Gosick"" token=""shows-portraits"" itemprop=""url"" href=""/gosick"" class=""text-link ellipsis"">
      Gosick    </a>
  </li>
                                  <li id=""media_group_178195"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""178195"">
    <a title=""Greathunt"" token=""shows-portraits"" itemprop=""url"" href=""/greathunt"" class=""text-link ellipsis"">
      Greathunt    </a>
  </li>
                            </ul>
                        <h3>H</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_58164"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""58164"">
    <a title=""H2O: Footprints in the Sand"" token=""shows-portraits"" itemprop=""url"" href=""/h2o-footprints-in-the-sand"" class=""text-link ellipsis"">
      H2O: Footprints in the Sand    </a>
  </li>
                                  <li id=""media_group_246614"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246614"">
    <a title=""Hakuoki Reimeiroku"" token=""shows-portraits"" itemprop=""url"" href=""/hakuoki-reimeiroku"" class=""text-link ellipsis"">
      Hakuoki Reimeiroku    </a>
  </li>
                                  <li id=""media_group_199614"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""199614"">
    <a title=""Hanamaru Kindergarten"" token=""shows-portraits"" itemprop=""url"" href=""/hanamaru-kindergarten"" class=""text-link ellipsis"">
      Hanamaru Kindergarten    </a>
  </li>
                                  <li id=""media_group_180899"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""180899"">
    <a title=""Hanasakeru Seishonen"" token=""shows-portraits"" itemprop=""url"" href=""/hanasakeru-seishonen"" class=""text-link ellipsis"">
      Hanasakeru Seishonen    </a>
  </li>
                                  <li id=""media_group_229038"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229038"">
    <a title=""Hanasaku Iroha"" token=""shows-portraits"" itemprop=""url"" href=""/hanasaku-iroha"" class=""text-link ellipsis"">
      Hanasaku Iroha    </a>
  </li>
                                  <li id=""media_group_47130"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47130"">
    <a title=""Happiness!"" token=""shows-portraits"" itemprop=""url"" href=""/happiness"" class=""text-link ellipsis"">
      Happiness!    </a>
  </li>
                                  <li id=""media_group_52008"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""52008"">
    <a title=""Hayate no Gotoku"" token=""shows-portraits"" itemprop=""url"" href=""/hayate-no-gotoku"" class=""text-link ellipsis"">
      Hayate no Gotoku    </a>
  </li>
                                  <li id=""media_group_245916"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""245916"">
    <a title=""Hell Girl: Two Mirrors"" token=""shows-portraits"" itemprop=""url"" href=""/hell-girl-two-mirrors"" class=""text-link ellipsis"">
      Hell Girl: Two Mirrors    </a>
  </li>
                                  <li id=""media_group_206376"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""206376"">
    <a title=""HEROMAN"" token=""shows-portraits"" itemprop=""url"" href=""/heroman"" class=""text-link ellipsis"">
      HEROMAN    </a>
  </li>
                                  <li id=""media_group_243068"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243068"">
    <a title=""Hiiro No Kakera"" token=""shows-portraits"" itemprop=""url"" href=""/hiiro-no-kakera"" class=""text-link ellipsis"">
      Hiiro No Kakera    </a>
  </li>
                                  <li id=""media_group_238470"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""238470"">
    <a title=""Horizon in the Middle of Nowhere"" token=""shows-portraits"" itemprop=""url"" href=""/horizon-in-the-middle-of-nowhere"" class=""text-link ellipsis"">
      Horizon in the Middle of Nowhere    </a>
  </li>
                                  <li id=""media_group_224830"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""224830"">
    <a title=""Hourou Musuko Wandering Son"" token=""shows-portraits"" itemprop=""url"" href=""/hourou-musuko-wandering-son"" class=""text-link ellipsis"">
      Hourou Musuko Wandering Son    </a>
  </li>
                                  <li id=""media_group_246860"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246860"">
    <a title=""Humanity Has Declined"" token=""shows-portraits"" itemprop=""url"" href=""/humanity-has-declined"" class=""text-link ellipsis"">
      Humanity Has Declined    </a>
  </li>
                                  <li id=""media_group_237800"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""237800"">
    <a title=""Hunter x Hunter"" token=""shows-portraits"" itemprop=""url"" href=""/hunter-x-hunter"" class=""text-link ellipsis"">
      Hunter x Hunter    </a>
  </li>
                            </ul>
                        <h3>I</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_46628"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""46628"">
    <a title=""Ikkitousen"" token=""shows-portraits"" itemprop=""url"" href=""/ikkitousen"" class=""text-link ellipsis"">
      Ikkitousen    </a>
  </li>
                                  <li id=""media_group_230336"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""230336"">
    <a title=""Interlude"" token=""shows-portraits"" itemprop=""url"" href=""/interlude"" class=""text-link ellipsis"">
      Interlude    </a>
  </li>
                                  <li id=""media_group_240612"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240612"">
    <a title=""Inu X Boku Secret Service"" token=""shows-portraits"" itemprop=""url"" href=""/inu-x-boku-secret-service"" class=""text-link ellipsis"">
      Inu X Boku Secret Service    </a>
  </li>
                            </ul>
                        <h3>J</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_179178"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""179178"">
    <a title=""Japan Tourism Anime Channel"" token=""shows-portraits"" itemprop=""url"" href=""/japan-tourism-anime-channel"" class=""text-link ellipsis"">
      Japan Tourism Anime Channel    </a>
  </li>
                                  <li id=""media_group_223141"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""223141"">
    <a title=""Japancast"" token=""shows-portraits"" itemprop=""url"" href=""/japancast"" class=""text-link ellipsis"">
      Japancast    </a>
  </li>
                                  <li id=""media_group_151168"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""151168"">
    <a title=""Japanese Anime Classic Collection 1"" token=""shows-portraits"" itemprop=""url"" href=""/japanese-anime-classic-collection-1"" class=""text-link ellipsis"">
      Japanese Anime Classic Collection 1    </a>
  </li>
                                  <li id=""media_group_151172"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""151172"">
    <a title=""Japanese Anime Classic Collection 3"" token=""shows-portraits"" itemprop=""url"" href=""/japanese-anime-classic-collection-3"" class=""text-link ellipsis"">
      Japanese Anime Classic Collection 3    </a>
  </li>
                                  <li id=""media_group_151174"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""151174"">
    <a title=""Japanese Anime Classic Collection 4"" token=""shows-portraits"" itemprop=""url"" href=""/japanese-anime-classic-collection-4"" class=""text-link ellipsis"">
      Japanese Anime Classic Collection 4    </a>
  </li>
                            </ul>
                        <h3>K</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_168263"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""168263"">
    <a title=""Kaasan Mom's Life"" token=""shows-portraits"" itemprop=""url"" href=""/kaasan-moms-life"" class=""text-link ellipsis"">
      Kaasan Mom's Life    </a>
  </li>
                                  <li id=""media_group_150701"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""150701"">
    <a title=""Kaede New Town"" token=""shows-portraits"" itemprop=""url"" href=""/kaede-new-town"" class=""text-link ellipsis"">
      Kaede New Town    </a>
  </li>
                                  <li id=""media_group_234149"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234149"">
    <a title=""Kamisama Dolls"" token=""shows-portraits"" itemprop=""url"" href=""/kamisama-dolls"" class=""text-link ellipsis"">
      Kamisama Dolls    </a>
  </li>
                                  <li id=""media_group_179836"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""179836"">
    <a title=""Kanamemo"" token=""shows-portraits"" itemprop=""url"" href=""/kanamemo"" class=""text-link ellipsis"">
      Kanamemo    </a>
  </li>
                                  <li id=""media_group_62326"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""62326"">
    <a title=""Kanokon"" token=""shows-portraits"" itemprop=""url"" href=""/kanokon"" class=""text-link ellipsis"">
      Kanokon    </a>
  </li>
                                  <li id=""media_group_118236"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""118236"">
    <a title=""Kemeko DX"" token=""shows-portraits"" itemprop=""url"" href=""/kemeko-dx"" class=""text-link ellipsis"">
      Kemeko DX    </a>
  </li>
                                  <li id=""media_group_87570"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""87570"">
    <a title=""Kid Kosmo"" token=""shows-portraits"" itemprop=""url"" href=""/kid-kosmo"" class=""text-link ellipsis"">
      Kid Kosmo    </a>
  </li>
                                  <li id=""media_group_185112"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""185112"">
    <a title=""Kiddy GiRL-AND"" token=""shows-portraits"" itemprop=""url"" href=""/kiddy-girl-and"" class=""text-link ellipsis"">
      Kiddy GiRL-AND    </a>
  </li>
                                  <li id=""media_group_244298"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244298"">
    <a title=""Kids on the Slope"" token=""shows-portraits"" itemprop=""url"" href=""/kids-on-the-slope"" class=""text-link ellipsis"">
      Kids on the Slope    </a>
  </li>
                                  <li id=""media_group_168118"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""168118"">
    <a title=""Kigurumikku"" token=""shows-portraits"" itemprop=""url"" href=""/kigurumikku"" class=""text-link ellipsis"">
      Kigurumikku    </a>
  </li>
                                  <li id=""media_group_88348"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""88348"">
    <a title=""Kite Liberator"" token=""shows-portraits"" itemprop=""url"" href=""/kite-liberator"" class=""text-link ellipsis"">
      Kite Liberator    </a>
  </li>
                                  <li id=""media_group_228630"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""228630"">
    <a title=""Kobo chan"" token=""shows-portraits"" itemprop=""url"" href=""/kobo-chan"" class=""text-link ellipsis"">
      Kobo chan    </a>
  </li>
                                  <li id=""media_group_81052"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""81052"">
    <a title=""Koihime Musou"" token=""shows-portraits"" itemprop=""url"" href=""/koihime-musou"" class=""text-link ellipsis"">
      Koihime Musou    </a>
  </li>
                                  <li id=""media_group_246836"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246836"">
    <a title=""Kokoro Connect"" token=""shows-portraits"" itemprop=""url"" href=""/kokoro-connect"" class=""text-link ellipsis"">
      Kokoro Connect    </a>
  </li>
                                  <li id=""media_group_53158"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""53158"">
    <a title=""Kono Aozora ni Yakusoku wo"" token=""shows-portraits"" itemprop=""url"" href=""/kono-aozora-ni-yakusoku-wo"" class=""text-link ellipsis"">
      Kono Aozora ni Yakusoku wo    </a>
  </li>
                                  <li id=""media_group_174635"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""174635"">
    <a title=""Kurokami The Animation"" token=""shows-portraits"" itemprop=""url"" href=""/kurokami-the-animation"" class=""text-link ellipsis"">
      Kurokami The Animation    </a>
  </li>
                                  <li id=""media_group_243974"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243974"">
    <a title=""Kuroko's Basketball"" token=""shows-portraits"" itemprop=""url"" href=""/kurokos-basketball"" class=""text-link ellipsis"">
      Kuroko's Basketball    </a>
  </li>
                            </ul>
                        <h3>L</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_47258"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47258"">
    <a title=""La Corda d'Oro ~primo passo~ and ~secondo passo~"" token=""shows-portraits"" itemprop=""url"" href=""/la-corda-doro-primo-passo-and-secondo-passo"" class=""text-link ellipsis"">
      La Corda d'Oro ~primo passo~ and ~secondo passo~    </a>
  </li>
                                  <li id=""media_group_119420"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""119420"">
    <a title=""La Maison en Petits Cubes"" token=""shows-portraits"" itemprop=""url"" href=""/la-maison-en-petits-cubes"" class=""text-link ellipsis"">
      La Maison en Petits Cubes    </a>
  </li>
                                  <li id=""media_group_246838"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246838"">
    <a title=""La storia della Arcana Famiglia"" token=""shows-portraits"" itemprop=""url"" href=""/la-storia-della-arcana-famiglia"" class=""text-link ellipsis"">
      La storia della Arcana Famiglia    </a>
  </li>
                                  <li id=""media_group_89610"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""89610"">
    <a title=""Lady Death"" token=""shows-portraits"" itemprop=""url"" href=""/lady-death"" class=""text-link ellipsis"">
      Lady Death    </a>
  </li>
                                  <li id=""media_group_243602"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243602"">
    <a title=""Leiji Matsumoto's OZMA"" token=""shows-portraits"" itemprop=""url"" href=""/leiji-matsumotos-ozma"" class=""text-link ellipsis"">
      Leiji Matsumoto's OZMA    </a>
  </li>
                                  <li id=""media_group_224791"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""224791"">
    <a title=""Level E"" token=""shows-portraits"" itemprop=""url"" href=""/level-e"" class=""text-link ellipsis"">
      Level E    </a>
  </li>
                                  <li id=""media_group_206590"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""206590"">
    <a title=""Lilpri"" token=""shows-portraits"" itemprop=""url"" href=""/lilpri"" class=""text-link ellipsis"">
      Lilpri    </a>
  </li>
                                  <li id=""media_group_96409"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""96409"">
    <a title=""Linebarrels of Iron"" token=""shows-portraits"" itemprop=""url"" href=""/linebarrels-of-iron"" class=""text-link ellipsis"">
      Linebarrels of Iron    </a>
  </li>
                                  <li id=""media_group_240616"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240616"">
    <a title=""Listen to Me, Girls, I’m Your Father!"" token=""shows-portraits"" itemprop=""url"" href=""/listen-to-me-girls-im-your-father"" class=""text-link ellipsis"">
      Listen to Me, Girls, I’m Your Father!    </a>
  </li>
                                  <li id=""media_group_239534"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""239534"">
    <a title=""Little Nemo: Adventures in Slumberland"" token=""shows-portraits"" itemprop=""url"" href=""/little-nemo-adventures-in-slumberland"" class=""text-link ellipsis"">
      Little Nemo: Adventures in Slumberland    </a>
  </li>
                                  <li id=""media_group_52148"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""52148"">
    <a title=""Lucky Star"" token=""shows-portraits"" itemprop=""url"" href=""/lucky-star"" class=""text-link ellipsis"">
      Lucky Star    </a>
  </li>
                            </ul>
                        <h3>M</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_47712"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47712"">
    <a title=""Maburaho"" token=""shows-portraits"" itemprop=""url"" href=""/maburaho"" class=""text-link ellipsis"">
      Maburaho    </a>
  </li>
                                  <li id=""media_group_89618"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""89618"">
    <a title=""Magical Play"" token=""shows-portraits"" itemprop=""url"" href=""/magical-play"" class=""text-link ellipsis"">
      Magical Play    </a>
  </li>
                                  <li id=""media_group_222071"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""222071"">
    <a title=""Magicians Academy"" token=""shows-portraits"" itemprop=""url"" href=""/magicians-academy"" class=""text-link ellipsis"">
      Magicians Academy    </a>
  </li>
                            </ul>
                      </div>
                    <div class=""videos-column left"">
                        <h3>M<span class=""small-data""> continued</span></h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_238020"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""238020"">
    <a title=""Majikoi Oh! Samurai Girls"" token=""shows-portraits"" itemprop=""url"" href=""/majikoi-oh-samurai-girls"" class=""text-link ellipsis"">
      Majikoi Oh! Samurai Girls    </a>
  </li>
                                  <li id=""media_group_238580"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""238580"">
    <a title=""Mashiroiro Symphony"" token=""shows-portraits"" itemprop=""url"" href=""/mashiroiro-symphony"" class=""text-link ellipsis"">
      Mashiroiro Symphony    </a>
  </li>
                                  <li id=""media_group_244228"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244228"">
    <a title=""Medaka Box"" token=""shows-portraits"" itemprop=""url"" href=""/medaka-box"" class=""text-link ellipsis"">
      Medaka Box    </a>
  </li>
                                  <li id=""media_group_87578"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""87578"">
    <a title=""Mighty Max"" token=""shows-portraits"" itemprop=""url"" href=""/mighty-max"" class=""text-link ellipsis"">
      Mighty Max    </a>
  </li>
                                  <li id=""media_group_192183"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""192183"">
    <a title=""Miracle Train"" token=""shows-portraits"" itemprop=""url"" href=""/miracle-train"" class=""text-link ellipsis"">
      Miracle Train    </a>
  </li>
                                  <li id=""media_group_214727"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""214727"">
    <a title=""Mitsudomoe"" token=""shows-portraits"" itemprop=""url"" href=""/mitsudomoe"" class=""text-link ellipsis"">
      Mitsudomoe    </a>
  </li>
                                  <li id=""media_group_53762"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""53762"">
    <a title=""Mizu no Kotoba"" token=""shows-portraits"" itemprop=""url"" href=""/mizu-no-kotoba"" class=""text-link ellipsis"">
      Mizu no Kotoba    </a>
  </li>
                                  <li id=""media_group_200143"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""200143"">
    <a title=""Mobile Suit Gundam"" token=""shows-portraits"" itemprop=""url"" href=""/mobile-suit-gundam"" class=""text-link ellipsis"">
      Mobile Suit Gundam    </a>
  </li>
                                  <li id=""media_group_172756"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""172756"">
    <a title=""MOBILE SUIT GUNDAM 00"" token=""shows-portraits"" itemprop=""url"" href=""/mobile-suit-gundam-00"" class=""text-link ellipsis"">
      MOBILE SUIT GUNDAM 00    </a>
  </li>
                                  <li id=""media_group_200853"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""200853"">
    <a title=""Mobile Suit Gundam UC (Unicorn)"" token=""shows-portraits"" itemprop=""url"" href=""/mobile-suit-gundam-uc-unicorn"" class=""text-link ellipsis"">
      Mobile Suit Gundam UC (Unicorn)    </a>
  </li>
                                  <li id=""media_group_115438"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""115438"">
    <a title=""Mobile Suit Gundam Wing"" token=""shows-portraits"" itemprop=""url"" href=""/mobile-suit-gundam-wing"" class=""text-link ellipsis"">
      Mobile Suit Gundam Wing    </a>
  </li>
                                  <li id=""media_group_115432"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""115432"">
    <a title=""Mobile Suit Zeta Gundam"" token=""shows-portraits"" itemprop=""url"" href=""/mobile-suit-zeta-gundam"" class=""text-link ellipsis"">
      Mobile Suit Zeta Gundam    </a>
  </li>
                                  <li id=""media_group_225580"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""225580"">
    <a title=""Moribito"" token=""shows-portraits"" itemprop=""url"" href=""/moribito"" class=""text-link ellipsis"">
      Moribito    </a>
  </li>
                                  <li id=""media_group_235340"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""235340"">
    <a title=""Moritasan wa Mukuchi"" token=""shows-portraits"" itemprop=""url"" href=""/moritasan-wa-mukuchi"" class=""text-link ellipsis"">
      Moritasan wa Mukuchi    </a>
  </li>
                                  <li id=""media_group_47486"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47486"">
    <a title=""Mouse"" token=""shows-portraits"" itemprop=""url"" href=""/mouse"" class=""text-link ellipsis"">
      Mouse    </a>
  </li>
                                  <li id=""media_group_247118"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""247118"">
    <a title=""Moyashimon"" token=""shows-portraits"" itemprop=""url"" href=""/moyashimon"" class=""text-link ellipsis"">
      Moyashimon    </a>
  </li>
                                  <li id=""media_group_50538"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""50538"">
    <a title=""Mushi-Uta"" token=""shows-portraits"" itemprop=""url"" href=""/mushi-uta"" class=""text-link ellipsis"">
      Mushi-Uta    </a>
  </li>
                                  <li id=""media_group_229046"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229046"">
    <a title=""My Ordinary Life"" token=""shows-portraits"" itemprop=""url"" href=""/my-ordinary-life"" class=""text-link ellipsis"">
      My Ordinary Life    </a>
  </li>
                                  <li id=""media_group_55898"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""55898"">
    <a title=""Myself; Yourself"" token=""shows-portraits"" itemprop=""url"" href=""/myself-yourself"" class=""text-link ellipsis"">
      Myself; Yourself    </a>
  </li>
                                  <li id=""media_group_244056"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244056"">
    <a title=""Mysterious Girlfriend X"" token=""shows-portraits"" itemprop=""url"" href=""/mysterious-girlfriend-x"" class=""text-link ellipsis"">
      Mysterious Girlfriend X    </a>
  </li>
                            </ul>
                        <h3>N</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_247344"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""247344"">
    <a title=""Nakaimo - My Sister is Among Them!"" token=""shows-portraits"" itemprop=""url"" href=""/nakaimo-my-sister-is-among-them"" class=""text-link ellipsis"">
      Nakaimo - My Sister is Among Them!    </a>
  </li>
                                  <li id=""media_group_189368"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""189368"">
    <a title=""Naked Wolves"" token=""shows-portraits"" itemprop=""url"" href=""/naked-wolves"" class=""text-link ellipsis"">
      Naked Wolves    </a>
  </li>
                                  <li id=""media_group_42850"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""42850"">
    <a title=""Naruto"" token=""shows-portraits"" itemprop=""url"" href=""/naruto"" class=""text-link ellipsis"">
      Naruto    </a>
  </li>
                                  <li id=""media_group_42852"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""42852"">
    <a title=""Naruto Shippuden"" token=""shows-portraits"" itemprop=""url"" href=""/naruto-shippuden"" class=""text-link ellipsis"">
      Naruto Shippuden    </a>
  </li>
                                  <li id=""media_group_244046"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244046"">
    <a title=""NARUTO Spin-Off: Rock Lee &amp; His Ninja Pals"" token=""shows-portraits"" itemprop=""url"" href=""/naruto-spin-off-rock-lee-his-ninja-pals"" class=""text-link ellipsis"">
      NARUTO Spin-Off: Rock Lee &amp; His Ninja Pals    </a>
  </li>
                                  <li id=""media_group_169063"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""169063"">
    <a title=""NATSU NO ARASHI !"" token=""shows-portraits"" itemprop=""url"" href=""/natsu-no-arashi-"" class=""text-link ellipsis"">
      NATSU NO ARASHI !    </a>
  </li>
                                  <li id=""media_group_81875"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""81875"">
    <a title=""Natsume Yujin-cho"" token=""shows-portraits"" itemprop=""url"" href=""/natsume-yujin-cho"" class=""text-link ellipsis"">
      Natsume Yujin-cho    </a>
  </li>
                                  <li id=""media_group_241846"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""241846"">
    <a title=""Natsume Yujin-cho Shi"" token=""shows-portraits"" itemprop=""url"" href=""/natsume-yujin-cho-shi"" class=""text-link ellipsis"">
      Natsume Yujin-cho Shi    </a>
  </li>
                                  <li id=""media_group_246682"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246682"">
    <a title=""Natsuyuki Rendezvous"" token=""shows-portraits"" itemprop=""url"" href=""/natsuyuki-rendezvous"" class=""text-link ellipsis"">
      Natsuyuki Rendezvous    </a>
  </li>
                                  <li id=""media_group_62522"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""62522"">
    <a title=""Neo Angelique Abyss"" token=""shows-portraits"" itemprop=""url"" href=""/neo-angelique-abyss"" class=""text-link ellipsis"">
      Neo Angelique Abyss    </a>
  </li>
                                  <li id=""media_group_245920"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""245920"">
    <a title=""Night Raid 1931"" token=""shows-portraits"" itemprop=""url"" href=""/night-raid-1931"" class=""text-link ellipsis"">
      Night Raid 1931    </a>
  </li>
                                  <li id=""media_group_240610"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240610"">
    <a title=""Nisemonogatari"" token=""shows-portraits"" itemprop=""url"" href=""/nisemonogatari"" class=""text-link ellipsis"">
      Nisemonogatari    </a>
  </li>
                                  <li id=""media_group_234107"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234107"">
    <a title=""No. 6"" token=""shows-portraits"" itemprop=""url"" href=""/no-6"" class=""text-link ellipsis"">
      No. 6    </a>
  </li>
                                  <li id=""media_group_241436"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""241436"">
    <a title=""Nyarko-san: Another Crawling Chaos"" token=""shows-portraits"" itemprop=""url"" href=""/nyarko-san-another-crawling-chaos"" class=""text-link ellipsis"">
      Nyarko-san: Another Crawling Chaos    </a>
  </li>
                            </ul>
                        <h3>O</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_212723"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""212723"">
    <a title=""Occult Academy"" token=""shows-portraits"" itemprop=""url"" href=""/occult-academy"" class=""text-link ellipsis"">
      Occult Academy    </a>
  </li>
                                  <li id=""media_group_199612"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""199612"">
    <a title=""Omamori Himari"" token=""shows-portraits"" itemprop=""url"" href=""/omamori-himari"" class=""text-link ellipsis"">
      Omamori Himari    </a>
  </li>
                                  <li id=""media_group_236074"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""236074"">
    <a title=""Oreimo"" token=""shows-portraits"" itemprop=""url"" href=""/oreimo"" class=""text-link ellipsis"">
      Oreimo    </a>
  </li>
                                  <li id=""media_group_226838"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""226838"">
    <a title=""Otaku No Video Podcast"" token=""shows-portraits"" itemprop=""url"" href=""/otaku-no-video-podcast"" class=""text-link ellipsis"">
      Otaku No Video Podcast    </a>
  </li>
                                  <li id=""media_group_223212"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""223212"">
    <a title=""Otaku-Verse Zero"" token=""shows-portraits"" itemprop=""url"" href=""/otaku-verse-zero"" class=""text-link ellipsis"">
      Otaku-Verse Zero    </a>
  </li>
                                  <li id=""media_group_220642"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""220642"">
    <a title=""Otome Yokai Zakuro"" token=""shows-portraits"" itemprop=""url"" href=""/otome-yokai-zakuro"" class=""text-link ellipsis"">
      Otome Yokai Zakuro    </a>
  </li>
                            </ul>
                        <h3>P</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_48994"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""48994"">
    <a title=""Pale Cocoon"" token=""shows-portraits"" itemprop=""url"" href=""/pale-cocoon"" class=""text-link ellipsis"">
      Pale Cocoon    </a>
  </li>
                                  <li id=""media_group_89628"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""89628"">
    <a title=""Parasite Dolls"" token=""shows-portraits"" itemprop=""url"" href=""/parasite-dolls"" class=""text-link ellipsis"">
      Parasite Dolls    </a>
  </li>
                                  <li id=""media_group_167129"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""167129"">
    <a title=""Peeping Life"" token=""shows-portraits"" itemprop=""url"" href=""/peeping-life"" class=""text-link ellipsis"">
      Peeping Life    </a>
  </li>
                                  <li id=""media_group_232188"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""232188"">
    <a title=""Persona"" token=""shows-portraits"" itemprop=""url"" href=""/persona"" class=""text-link ellipsis"">
      Persona    </a>
  </li>
                                  <li id=""media_group_247000"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""247000"">
    <a title=""PES: Peace Eco Smile"" token=""shows-portraits"" itemprop=""url"" href=""/pes-peace-eco-smile"" class=""text-link ellipsis"">
      PES: Peace Eco Smile    </a>
  </li>
                                  <li id=""media_group_201213"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""201213"">
    <a title=""Phantom Thief Reinya"" token=""shows-portraits"" itemprop=""url"" href=""/phantom-thief-reinya"" class=""text-link ellipsis"">
      Phantom Thief Reinya    </a>
  </li>
                                  <li id=""media_group_244288"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244288"">
    <a title=""Phi Brain"" token=""shows-portraits"" itemprop=""url"" href=""/phi-brain"" class=""text-link ellipsis"">
      Phi Brain    </a>
  </li>
                                  <li id=""media_group_243968"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243968"">
    <a title=""Polar Bear Cafe"" token=""shows-portraits"" itemprop=""url"" href=""/polar-bear-cafe"" class=""text-link ellipsis"">
      Polar Bear Cafe    </a>
  </li>
                                  <li id=""media_group_240606"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240606"">
    <a title=""Poyopoyo"" token=""shows-portraits"" itemprop=""url"" href=""/poyopoyo"" class=""text-link ellipsis"">
      Poyopoyo    </a>
  </li>
                                  <li id=""media_group_109008"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""109008"">
    <a title=""Pretty Cure"" token=""shows-portraits"" itemprop=""url"" href=""/pretty-cure"" class=""text-link ellipsis"">
      Pretty Cure    </a>
  </li>
                                  <li id=""media_group_242294"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""242294"">
    <a title=""Puella Magi Madoka Magica"" token=""shows-portraits"" itemprop=""url"" href=""/puella-magi-madoka-magica"" class=""text-link ellipsis"">
      Puella Magi Madoka Magica    </a>
  </li>
                            </ul>
                        <h3>Q</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_119010"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""119010"">
    <a title=""Queen's Blade"" token=""shows-portraits"" itemprop=""url"" href=""/queens-blade"" class=""text-link ellipsis"">
      Queen's Blade    </a>
  </li>
                                  <li id=""media_group_244420"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244420"">
    <a title=""Queen's Blade Rebellion"" token=""shows-portraits"" itemprop=""url"" href=""/queens-blade-rebellion"" class=""text-link ellipsis"">
      Queen's Blade Rebellion    </a>
  </li>
                            </ul>
                        <h3>R</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_234111"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234111"">
    <a title=""R-15"" token=""shows-portraits"" itemprop=""url"" href=""/r-15"" class=""text-link ellipsis"">
      R-15    </a>
  </li>
                                  <li id=""media_group_88344"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""88344"">
    <a title=""Ramen Fighter Miki"" token=""shows-portraits"" itemprop=""url"" href=""/ramen-fighter-miki"" class=""text-link ellipsis"">
      Ramen Fighter Miki    </a>
  </li>
                                  <li id=""media_group_47700"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47700"">
    <a title=""REBORN!"" token=""shows-portraits"" itemprop=""url"" href=""/reborn"" class=""text-link ellipsis"">
      REBORN!    </a>
  </li>
                                  <li id=""media_group_240620"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240620"">
    <a title=""Recorder and Randsell"" token=""shows-portraits"" itemprop=""url"" href=""/recorder-and-randsell"" class=""text-link ellipsis"">
      Recorder and Randsell    </a>
  </li>
                                  <li id=""media_group_224835"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""224835"">
    <a title=""Rio Rainbow Gate"" token=""shows-portraits"" itemprop=""url"" href=""/rio-rainbow-gate"" class=""text-link ellipsis"">
      Rio Rainbow Gate    </a>
  </li>
                            </ul>
                        <h3>S</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_234122"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234122"">
    <a title=""Sacred Seven"" token=""shows-portraits"" itemprop=""url"" href=""/sacred-seven"" class=""text-link ellipsis"">
      Sacred Seven    </a>
  </li>
                                  <li id=""media_group_244002"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244002"">
    <a title=""Saint Seiya Omega"" token=""shows-portraits"" itemprop=""url"" href=""/saint-seiya-omega"" class=""text-link ellipsis"">
      Saint Seiya Omega    </a>
  </li>
                                  <li id=""media_group_224831"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""224831"">
    <a title=""Saint Seiya The Lost Canvas"" token=""shows-portraits"" itemprop=""url"" href=""/saint-seiya-the-lost-canvas"" class=""text-link ellipsis"">
      Saint Seiya The Lost Canvas    </a>
  </li>
                                  <li id=""media_group_130082"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""130082"">
    <a title=""Saki"" token=""shows-portraits"" itemprop=""url"" href=""/saki"" class=""text-link ellipsis"">
      Saki    </a>
  </li>
                                  <li id=""media_group_243976"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243976"">
    <a title=""Saki Episode of Side A"" token=""shows-portraits"" itemprop=""url"" href=""/saki-episode-of-side-a"" class=""text-link ellipsis"">
      Saki Episode of Side A    </a>
  </li>
                                  <li id=""media_group_192644"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""192644"">
    <a title=""Sasameki Koto"" token=""shows-portraits"" itemprop=""url"" href=""/sasameki-koto"" class=""text-link ellipsis"">
      Sasameki Koto    </a>
  </li>
                                  <li id=""media_group_170864"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""170864"">
    <a title=""School Days"" token=""shows-portraits"" itemprop=""url"" href=""/school-days"" class=""text-link ellipsis"">
      School Days    </a>
  </li>
                                  <li id=""media_group_185412"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""185412"">
    <a title=""Seitokai No Ichizon - Student Councils Discretion"" token=""shows-portraits"" itemprop=""url"" href=""/seitokai-no-ichizon-student-councils-discretion"" class=""text-link ellipsis"">
      Seitokai No Ichizon - Student Councils Discretion    </a>
  </li>
                                  <li id=""media_group_229042"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229042"">
    <a title=""Sekai Ichi Hatsukoi - World's Greatest First Love"" token=""shows-portraits"" itemprop=""url"" href=""/sekai-ichi-hatsukoi-worlds-greatest-first-love"" class=""text-link ellipsis"">
      Sekai Ichi Hatsukoi - World's Greatest First Love    </a>
  </li>
                                  <li id=""media_group_243970"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243970"">
    <a title=""Sengoku Collection (Parallel World Samurai)"" token=""shows-portraits"" itemprop=""url"" href=""/sengoku-collection-parallel-world-samurai"" class=""text-link ellipsis"">
      Sengoku Collection (Parallel World Samurai)    </a>
  </li>
                                  <li id=""media_group_179111"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""179111"">
    <a title=""Sherlock Hound"" token=""shows-portraits"" itemprop=""url"" href=""/sherlock-hound"" class=""text-link ellipsis"">
      Sherlock Hound    </a>
  </li>
                                  <li id=""media_group_192243"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""192243"">
    <a title=""Shin Koihime Musou"" token=""shows-portraits"" itemprop=""url"" href=""/shin-koihime-musou"" class=""text-link ellipsis"">
      Shin Koihime Musou    </a>
  </li>
                                  <li id=""media_group_206451"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""206451"">
    <a title=""Shin Koihime Musou - Otome Tairan"" token=""shows-portraits"" itemprop=""url"" href=""/shin-koihime-musou-otome-tairan"" class=""text-link ellipsis"">
      Shin Koihime Musou - Otome Tairan    </a>
  </li>
                                  <li id=""media_group_243318"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243318"">
    <a title=""SHOTanime"" token=""shows-portraits"" itemprop=""url"" href=""/shotanime"" class=""text-link ellipsis"">
      SHOTanime    </a>
  </li>
                                  <li id=""media_group_42860"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""42860"">
    <a title=""Shugo Chara"" token=""shows-portraits"" itemprop=""url"" href=""/shugo-chara"" class=""text-link ellipsis"">
      Shugo Chara    </a>
  </li>
                                  <li id=""media_group_89637"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""89637"">
    <a title=""Sin The Movie"" token=""shows-portraits"" itemprop=""url"" href=""/sin-the-movie"" class=""text-link ellipsis"">
      Sin The Movie    </a>
  </li>
                                  <li id=""media_group_229060"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229060"">
    <a title=""SKET Dance"" token=""shows-portraits"" itemprop=""url"" href=""/sket-dance"" class=""text-link ellipsis"">
      SKET Dance    </a>
  </li>
                                  <li id=""media_group_182853"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""182853"">
    <a title=""Sketchbook"" token=""shows-portraits"" itemprop=""url"" href=""/sketchbook"" class=""text-link ellipsis"">
      Sketchbook    </a>
  </li>
                                  <li id=""media_group_105505"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""105505"">
    <a title=""Skip Beat!"" token=""shows-portraits"" itemprop=""url"" href=""/skip-beat"" class=""text-link ellipsis"">
      Skip Beat!    </a>
  </li>
                                  <li id=""media_group_47822"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""47822"">
    <a title=""Slam Dunk"" token=""shows-portraits"" itemprop=""url"" href=""/slam-dunk"" class=""text-link ellipsis"">
      Slam Dunk    </a>
  </li>
                                  <li id=""media_group_246612"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246612"">
    <a title=""So, I Can't Play H!"" token=""shows-portraits"" itemprop=""url"" href=""/so-i-cant-play-h"" class=""text-link ellipsis"">
      So, I Can't Play H!    </a>
  </li>
                                  <li id=""media_group_199622"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""199622"">
    <a title=""Soranowoto"" token=""shows-portraits"" itemprop=""url"" href=""/soranowoto"" class=""text-link ellipsis"">
      Soranowoto    </a>
  </li>
                                  <li id=""media_group_243964"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243964"">
    <a title=""Space Brothers"" token=""shows-portraits"" itemprop=""url"" href=""/space-brothers"" class=""text-link ellipsis"">
      Space Brothers    </a>
  </li>
                                  <li id=""media_group_62092"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""62092"">
    <a title=""Speed Racer"" token=""shows-portraits"" itemprop=""url"" href=""/speed-racer"" class=""text-link ellipsis"">
      Speed Racer    </a>
  </li>
                                  <li id=""media_group_219881"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""219881"">
    <a title=""Squid Girl"" token=""shows-portraits"" itemprop=""url"" href=""/squid-girl"" class=""text-link ellipsis"">
      Squid Girl    </a>
  </li>
                                  <li id=""media_group_236076"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""236076"">
    <a title=""Star Driver"" token=""shows-portraits"" itemprop=""url"" href=""/star-driver"" class=""text-link ellipsis"">
      Star Driver    </a>
  </li>
                                  <li id=""media_group_130084"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""130084"">
    <a title=""Steel Angel Kurumi"" token=""shows-portraits"" itemprop=""url"" href=""/steel-angel-kurumi"" class=""text-link ellipsis"">
      Steel Angel Kurumi    </a>
  </li>
                                  <li id=""media_group_144882"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""144882"">
    <a title=""Steel Angel Kurumi 2 Shiki"" token=""shows-portraits"" itemprop=""url"" href=""/steel-angel-kurumi-2-shiki"" class=""text-link ellipsis"">
      Steel Angel Kurumi 2 Shiki    </a>
  </li>
                                  <li id=""media_group_53620"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""53620"">
    <a title=""Steel Angel Kurumi Zero"" token=""shows-portraits"" itemprop=""url"" href=""/steel-angel-kurumi-zero"" class=""text-link ellipsis"">
      Steel Angel Kurumi Zero    </a>
  </li>
                                  <li id=""media_group_229050"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229050"">
    <a title=""STEINS;GATE"" token=""shows-portraits"" itemprop=""url"" href=""/steinsgate"" class=""text-link ellipsis"">
      STEINS;GATE    </a>
  </li>
                                  <li id=""media_group_110666"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""110666"">
    <a title=""Sumiko"" token=""shows-portraits"" itemprop=""url"" href=""/sumiko"" class=""text-link ellipsis"">
      Sumiko    </a>
  </li>
                                  <li id=""media_group_219963"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""219963"">
    <a title=""Super Robot Wars OG The Inspector"" token=""shows-portraits"" itemprop=""url"" href=""/super-robot-wars-og-the-inspector"" class=""text-link ellipsis"">
      Super Robot Wars OG The Inspector    </a>
  </li>
                                  <li id=""media_group_246948"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246948"">
    <a title=""Sword Art Online"" token=""shows-portraits"" itemprop=""url"" href=""/sword-art-online"" class=""text-link ellipsis"">
      Sword Art Online    </a>
  </li>
                            </ul>
                        <h3>T</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_221268"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""221268"">
    <a title=""Tantei Opera Milky Holmes"" token=""shows-portraits"" itemprop=""url"" href=""/tantei-opera-milky-holmes"" class=""text-link ellipsis"">
      Tantei Opera Milky Holmes    </a>
  </li>
                                  <li id=""media_group_246840"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246840"">
    <a title=""Tari Tari"" token=""shows-portraits"" itemprop=""url"" href=""/tari-tari"" class=""text-link ellipsis"">
      Tari Tari    </a>
  </li>
                                  <li id=""media_group_191638"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""191638"">
    <a title=""Tegami Bachi Letter Bee"" token=""shows-portraits"" itemprop=""url"" href=""/tegami-bachi-letter-bee"" class=""text-link ellipsis"">
      Tegami Bachi Letter Bee    </a>
  </li>
                                  <li id=""media_group_246428"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246428"">
    <a title=""The Ambition of Oda Nobuna"" token=""shows-portraits"" itemprop=""url"" href=""/the-ambition-of-oda-nobuna"" class=""text-link ellipsis"">
      The Ambition of Oda Nobuna    </a>
  </li>
                                  <li id=""media_group_48808"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""48808"">
    <a title=""The Diary of Tortov Roddle"" token=""shows-portraits"" itemprop=""url"" href=""/the-diary-of-tortov-roddle"" class=""text-link ellipsis"">
      The Diary of Tortov Roddle    </a>
  </li>
                                  <li id=""media_group_234109"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234109"">
    <a title=""The Idol Master"" token=""shows-portraits"" itemprop=""url"" href=""/the-idol-master"" class=""text-link ellipsis"">
      The Idol Master    </a>
  </li>
                                  <li id=""media_group_240890"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240890"">
    <a title=""The Knight in the Area"" token=""shows-portraits"" itemprop=""url"" href=""/the-knight-in-the-area"" class=""text-link ellipsis"">
      The Knight in the Area    </a>
  </li>
                                  <li id=""media_group_245832"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""245832"">
    <a title=""The Legend of Qin"" token=""shows-portraits"" itemprop=""url"" href=""/the-legend-of-qin"" class=""text-link ellipsis"">
      The Legend of Qin    </a>
  </li>
                                  <li id=""media_group_239176"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""239176"">
    <a title=""The Live Show"" token=""shows-portraits"" itemprop=""url"" href=""/the-live-show"" class=""text-link ellipsis"">
      The Live Show    </a>
  </li>
                                  <li id=""media_group_46566"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""46566"">
    <a title=""The Melancholy of Haruhi Suzumiya"" token=""shows-portraits"" itemprop=""url"" href=""/the-melancholy-of-haruhi-suzumiya"" class=""text-link ellipsis"">
      The Melancholy of Haruhi Suzumiya    </a>
  </li>
                                  <li id=""media_group_124056"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""124056"">
    <a title=""The Melody of Oblivion"" token=""shows-portraits"" itemprop=""url"" href=""/the-melody-of-oblivion"" class=""text-link ellipsis"">
      The Melody of Oblivion    </a>
  </li>
                                  <li id=""media_group_234114"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234114"">
    <a title=""The Mystic Archives of Dantalian"" token=""shows-portraits"" itemprop=""url"" href=""/the-mystic-archives-of-dantalian"" class=""text-link ellipsis"">
      The Mystic Archives of Dantalian    </a>
  </li>
                                  <li id=""media_group_240608"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""240608"">
    <a title=""The Prince of Tennis II"" token=""shows-portraits"" itemprop=""url"" href=""/the-prince-of-tennis-ii"" class=""text-link ellipsis"">
      The Prince of Tennis II    </a>
  </li>
                                  <li id=""media_group_62208"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""62208"">
    <a title=""The Tower of DRUAGA -the Aegis of URUK-"" token=""shows-portraits"" itemprop=""url"" href=""/the-tower-of-druaga-the-aegis-of-uruk-"" class=""text-link ellipsis"">
      The Tower of DRUAGA -the Aegis of URUK-    </a>
  </li>
                                  <li id=""media_group_119016"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""119016"">
    <a title=""The Tower of DRUAGA -the Sword of URUK-"" token=""shows-portraits"" itemprop=""url"" href=""/the-tower-of-druaga-the-sword-of-uruk-"" class=""text-link ellipsis"">
      The Tower of DRUAGA -the Sword of URUK-    </a>
  </li>
                                  <li id=""media_group_219879"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""219879"">
    <a title=""The World God Only Knows"" token=""shows-portraits"" itemprop=""url"" href=""/the-world-god-only-knows"" class=""text-link ellipsis"">
      The World God Only Knows    </a>
  </li>
                                  <li id=""media_group_248190"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""248190"">
    <a title=""TheInsaneGamefreak"" token=""shows-portraits"" itemprop=""url"" href=""/theinsanegamefreak"" class=""text-link ellipsis"">
      TheInsaneGamefreak    </a>
  </li>
                                  <li id=""media_group_87731"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""87731"">
    <a title=""Time of Eve"" token=""shows-portraits"" itemprop=""url"" href=""/time-of-eve"" class=""text-link ellipsis"">
      Time of Eve    </a>
  </li>
                                  <li id=""media_group_213234"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""213234"">
    <a title=""Tono to Issho"" token=""shows-portraits"" itemprop=""url"" href=""/tono-to-issho"" class=""text-link ellipsis"">
      Tono to Issho    </a>
  </li>
                                  <li id=""media_group_246430"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246430"">
    <a title=""Total Eclipse"" token=""shows-portraits"" itemprop=""url"" href=""/total-eclipse"" class=""text-link ellipsis"">
      Total Eclipse    </a>
  </li>
                                  <li id=""media_group_248024"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""248024"">
    <a title=""Traveling Daru"" token=""shows-portraits"" itemprop=""url"" href=""/traveling-daru"" class=""text-link ellipsis"">
      Traveling Daru    </a>
  </li>
                                  <li id=""media_group_244256"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244256"">
    <a title=""Tsuritama"" token=""shows-portraits"" itemprop=""url"" href=""/tsuritama"" class=""text-link ellipsis"">
      Tsuritama    </a>
  </li>
                                  <li id=""media_group_234116"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234116"">
    <a title=""Twin Angel: Twinkle Paradise"" token=""shows-portraits"" itemprop=""url"" href=""/twin-angel-twinkle-paradise"" class=""text-link ellipsis"">
      Twin Angel: Twinkle Paradise    </a>
  </li>
                            </ul>
                        <h3>U</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_238468"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""238468"">
    <a title=""Un-Go"" token=""shows-portraits"" itemprop=""url"" href=""/un-go"" class=""text-link ellipsis"">
      Un-Go    </a>
  </li>
                                  <li id=""media_group_243972"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243972"">
    <a title=""Upotte!!"" token=""shows-portraits"" itemprop=""url"" href=""/upotte"" class=""text-link ellipsis"">
      Upotte!!    </a>
  </li>
                                  <li id=""media_group_206370"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""206370"">
    <a title=""Uraboku"" token=""shows-portraits"" itemprop=""url"" href=""/uraboku"" class=""text-link ellipsis"">
      Uraboku    </a>
  </li>
                                  <li id=""media_group_234151"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234151"">
    <a title=""Usagi Drop"" token=""shows-portraits"" itemprop=""url"" href=""/usagi-drop"" class=""text-link ellipsis"">
      Usagi Drop    </a>
  </li>
                                  <li id=""media_group_246966"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""246966"">
    <a title=""Utakoi"" token=""shows-portraits"" itemprop=""url"" href=""/utakoi"" class=""text-link ellipsis"">
      Utakoi    </a>
  </li>
                            </ul>
                        <h3>V</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_212079"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""212079"">
    <a title=""Valerian and Laureline"" token=""shows-portraits"" itemprop=""url"" href=""/valerian-and-laureline"" class=""text-link ellipsis"">
      Valerian and Laureline    </a>
  </li>
                                  <li id=""media_group_222048"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""222048"">
    <a title=""Venus to Mamoru!"" token=""shows-portraits"" itemprop=""url"" href=""/venus-to-mamoru"" class=""text-link ellipsis"">
      Venus to Mamoru!    </a>
  </li>
                                  <li id=""media_group_59086"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""59086"">
    <a title=""Viewtiful Joe"" token=""shows-portraits"" itemprop=""url"" href=""/viewtiful-joe"" class=""text-link ellipsis"">
      Viewtiful Joe    </a>
  </li>
                            </ul>
                        <h3>W</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_238016"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""238016"">
    <a title=""Wagnaria!!"" token=""shows-portraits"" itemprop=""url"" href=""/wagnaria"" class=""text-link ellipsis"">
      Wagnaria!!    </a>
  </li>
                                  <li id=""media_group_229051"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""229051"">
    <a title=""We, Without Wings - under the innocent sky"" token=""shows-portraits"" itemprop=""url"" href=""/we-without-wings-under-the-innocent-sky"" class=""text-link ellipsis"">
      We, Without Wings - under the innocent sky    </a>
  </li>
                                  <li id=""media_group_96462"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""96462"">
    <a title=""Web Ghosts PiPoPa"" token=""shows-portraits"" itemprop=""url"" href=""/web-ghosts-pipopa"" class=""text-link ellipsis"">
      Web Ghosts PiPoPa    </a>
  </li>
                                  <li id=""media_group_191567"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""191567"">
    <a title=""White Album"" token=""shows-portraits"" itemprop=""url"" href=""/white-album"" class=""text-link ellipsis"">
      White Album    </a>
  </li>
                                  <li id=""media_group_170741"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""170741"">
    <a title=""Wonder Beat Scramble"" token=""shows-portraits"" itemprop=""url"" href=""/wonder-beat-scramble"" class=""text-link ellipsis"">
      Wonder Beat Scramble    </a>
  </li>
                            </ul>
                        <h3>X</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_245922"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""245922"">
    <a title=""Xamd: Lost Memories"" token=""shows-portraits"" itemprop=""url"" href=""/xamd-lost-memories"" class=""text-link ellipsis"">
      Xamd: Lost Memories    </a>
  </li>
                            </ul>
                        <h3>Y</h3>
            <ul class=""clearfix medium-margin-bottom"">
                                <li id=""media_group_87576"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""87576"">
    <a title=""Yatler  Matler Space Tyrants"" token=""shows-portraits"" itemprop=""url"" href=""/yatler-matler-space-tyrants"" class=""text-link ellipsis"">
      Yatler  Matler Space Tyrants    </a>
  </li>
                                  <li id=""media_group_119458"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""119458"">
    <a title=""Yokuwakaru Gendai Maho"" token=""shows-portraits"" itemprop=""url"" href=""/yokuwakaru-gendai-maho"" class=""text-link ellipsis"">
      Yokuwakaru Gendai Maho    </a>
  </li>
                                  <li id=""media_group_48680"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""48680"">
    <a title=""Yonna in the Solitary Fortress"" token=""shows-portraits"" itemprop=""url"" href=""/yonna-in-the-solitary-fortress"" class=""text-link ellipsis"">
      Yonna in the Solitary Fortress    </a>
  </li>
                                  <li id=""media_group_237596"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""237596"">
    <a title=""You and Me"" token=""shows-portraits"" itemprop=""url"" href=""/you-and-me"" class=""text-link ellipsis"">
      You and Me    </a>
  </li>
                                  <li id=""media_group_243966"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""243966"">
    <a title=""You and Me 2"" token=""shows-portraits"" itemprop=""url"" href=""/you-and-me-2"" class=""text-link ellipsis"">
      You and Me 2    </a>
  </li>
                                  <li id=""media_group_244052"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""244052"">
    <a title=""Yurumates3Dei"" token=""shows-portraits"" itemprop=""url"" href=""/yurumates3dei"" class=""text-link ellipsis"">
      Yurumates3Dei    </a>
  </li>
                                  <li id=""media_group_234145"" itemscope itemtype=""http://schema.org/TVSeries"" class=""hover-bubble group-item"" group_id=""234145"">
    <a title=""YuruYuri"" token=""shows-portraits"" itemprop=""url"" href=""/yuruyuri"" class=""text-link ellipsis"">
      YuruYuri    </a>
  </li>
                            </ul>
                      </div>
                  </div>  
        
        
                </ul>
            <a href=""#"" class=""load-more button default-button large-button"" style=""display:none;"">More Shows</a>
    </div>
    
    <div id=""sidebar"" class=""right"">
      <ul id=""sidebar_elements"" class=""list-block"">
                <li class=""large-margin-bottom"">
          <h3>Try Free Trial</h3>
          <p class=""medium-margin-bottom"">No ads, full HD videos to your desktop, TV, and mobile devices.</p>
          <a href=""/freetrial/anime/?from=no_media_channel_mrec"" class=""text-link strong"">
            Start your 14-day free trial          </a>
        </li>
        <li class=""large-margin-bottom"">
          <div  class=""adtech_div"">
<iframe  width=""300"" height=""250"" scrolling=""no"" frameborder=""0"" marginheight=""0"" marginwidth=""0"" src=""http://adserver.adtechus.com/adiframe/3.0/5290/1285579/0/170/adtech;target=_blank;kvgender=F;kvage=32;kvpremium=0;grp=184372039""></iframe>
</div>        </li>
                <li class=""large-margin-bottom"">
          <h3>Featured Shows</h3>
          <ul class=""landscape-grid shows"">
                              <li itemscope itemtype=""http://schema.org/TVSeries"" class=""group-item"" group_id=""42852"">
      <div class=""hover-toggle-queue wrapper hover-classes"" data-classes=""container-shadow"">
        <a title=""Naruto Shippuden"" token=""shows-featured-landscapes"" itemprop=""url"" href=""/naruto-shippuden"" href=""#"" class=""landscape-element block-link cf titlefix"">
          <img itemprop=""photo"" alt=""Naruto Shippuden"" src=""http://img1.ak.crunchyroll.com/i/spire3/d028273200e2f0f4c7ca214873dce0fa1345229281_small.jpg"" class=""portrait medium-margin-right left"" />
          <div class=""series-info left"">
            <span itemprop=""name"" class=""series-title block ellipsis"">Naruto Shippuden</span>
            <span class=""series-data block"">
                              277 Videos                          </span>
          </div>
        </a>
      </div>
    </li>
                                <li itemscope itemtype=""http://schema.org/TVSeries"" class=""group-item"" group_id=""246842"">
      <div class=""hover-toggle-queue wrapper hover-classes"" data-classes=""container-shadow"">
        <a title=""Campione!"" token=""shows-featured-landscapes"" itemprop=""url"" href=""/campione"" href=""#"" class=""landscape-element block-link cf titlefix"">
          <img itemprop=""photo"" alt=""Campione!"" src=""http://img1.ak.crunchyroll.com/i/spire3/b875e1859893863a735e70051743c7f61341526670_small.jpg"" class=""portrait medium-margin-right left"" />
          <div class=""series-info left"">
            <span itemprop=""name"" class=""series-title block ellipsis"">Campione!</span>
            <span class=""series-data block"">
                              9 Videos                          </span>
          </div>
        </a>
      </div>
    </li>
                                <li itemscope itemtype=""http://schema.org/TVSeries"" class=""group-item"" group_id=""237800"">
      <div class=""hover-toggle-queue wrapper hover-classes"" data-classes=""container-shadow"">
        <a title=""Hunter x Hunter"" token=""shows-featured-landscapes"" itemprop=""url"" href=""/hunter-x-hunter"" href=""#"" class=""landscape-element block-link cf titlefix"">
          <img itemprop=""photo"" alt=""Hunter x Hunter"" src=""http://img1.ak.crunchyroll.com/i/spire4/15df7bc2e44c5b7d6dbb577b4af4e2d91342725613_small.jpg"" class=""portrait medium-margin-right left"" />
          <div class=""series-info left"">
            <span itemprop=""name"" class=""series-title block ellipsis"">Hunter x Hunter</span>
            <span class=""series-data block"">
                              44 Videos                          </span>
          </div>
        </a>
      </div>
    </li>
                                <li itemscope itemtype=""http://schema.org/TVSeries"" class=""group-item"" group_id=""246948"">
      <div class=""hover-toggle-queue wrapper hover-classes"" data-classes=""container-shadow"">
        <a title=""Sword Art Online"" token=""shows-featured-landscapes"" itemprop=""url"" href=""/sword-art-online"" href=""#"" class=""landscape-element block-link cf titlefix"">
          <img itemprop=""photo"" alt=""Sword Art Online"" src=""http://img1.ak.crunchyroll.com/i/spire4/5fe886f3daa6485efecf1ea6dc5207931343265548_small.jpg"" class=""portrait medium-margin-right left"" />
          <div class=""series-info left"">
            <span itemprop=""name"" class=""series-title block ellipsis"">Sword Art Online</span>
            <span class=""series-data block"">
                              8 Videos                          </span>
          </div>
        </a>
      </div>
    </li>
                                <li itemscope itemtype=""http://schema.org/TVSeries"" class=""group-item"" group_id=""42860"">
      <div class=""hover-toggle-queue wrapper hover-classes"" data-classes=""container-shadow"">
        <a title=""Shugo Chara"" token=""shows-featured-landscapes"" itemprop=""url"" href=""/shugo-chara"" href=""#"" class=""landscape-element block-link cf titlefix"">
          <img itemprop=""photo"" alt=""Shugo Chara"" src=""http://img1.ak.crunchyroll.com/i/spire1/7c0c0cd3958894b2dafc740ba847e4f11279141777_small.jpg"" class=""portrait medium-margin-right left"" />
          <div class=""series-info left"">
            <span itemprop=""name"" class=""series-title block ellipsis"">Shugo Chara</span>
            <span class=""series-data block"">
                              127 Videos                          </span>
          </div>
        </a>
      </div>
    </li>
                        </ul>
        </li>
        <li class=""large-margin-bottom"">
          <h3>Search by Genres</h3>
          <p class=""medium-margin-bottom"">Looking for something more specific? Try searching by genres.</p>
          <a href=""/videos/anime/genres"" class=""text-link strong search-by-genres"" token=""sidebar_genres"">
            Start searching by genres          </a>
        </li>
        <li class=""large-margin-bottom"">
          <h3>Follow Crunchyroll</h3>
          <p class=""medium-margin-bottom"">Get the latest updates on show information, news, and more.</p>
          <iframe src=""//www.facebook.com/plugins/like.php?href=http%3A%2F%2Fwww.facebook.com%2FCrunchyroll&amp;send=false&amp;layout=standard&amp;width=300&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font=lucida+grande&amp;height=35"" scrolling=""no"" frameborder=""0"" style=""border:none; overflow:hidden; width:300px; height:35px;"" allowTransparency=""true""></iframe>
                    <a href=""https://twitter.com/Crunchyroll"" class=""twitter-follow-button"" data-show-count=""false"">Follow @Crunchyroll</a>
<script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0];if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=""//platform.twitter.com/widgets.js"";fjs.parentNode.insertBefore(js,fjs);}}(document,""script"",""twitter-wjs"");</script>
                  </li>
      </ul>
    </div>
  </div>
</div>



<script id=""in_queue_template"" type=""text/x-jquery-tmpl"">
  <button class=""queue-button add-queue-button in-queue dark-button small-button button""
          id=""series_queue_${group_id}""
          group_id=""${group_id}"" type=""button"">
    <span class=""queue-icon""></span>
    <span class=""queue-label"">Remove</span>
  </button>
  <span class=""queue-indicator block""></span>
</script>

<script id=""not_queued_template"" type=""text/x-jquery-tmpl"">
  <button class=""queue-button add-queue-button not-queued special-button small-button button""
          id=""series_queue_${group_id}""
          group_id=""${group_id}"" type=""button"">
    <span class=""queue-icon""></span>
    <span class=""queue-label"">Add to Queue</span>
  </button>
</script>

<script id=""bubble_template"" type=""text/x-jquery-tmpl"">
  <div class=""portrait-bubble cf"">
    <div class=""tooltip-left left""></div>
    <div class=""portrait-desc container-shadow-dark left"">
      <span class=""series-title white ellipsis"">${name}</span>
      <p itemprop=""description"" class=""white"">
        {{if secondary_desc}}
        <span class=""secondary"">${secondary_desc}</span><br/>
        {{/if}}
        ${description}
      </p>
    </div>
  </div>
</script>

    <script type=""text/javascript"">
          $(""#media_group_170134"").data('bubble_data', {""name"":""07 Ghost"",""description"":""Teased unmercifully for his past as an orphan and a slave, Teito has only his best friend Mikage and his mastery of the magical art of Zaiphon to ease his days at the elite Barsburg Academy."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_192242"").data('bubble_data', {""name"":""11eyes"",""description"":""Satsuki Kakeru lived an ordinary life, until one day - with childhood friend Minase Yuka - they are transported to a different world."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229030"").data('bubble_data', {""name"":""A Bridge to the Starry Skies - Hoshizora e Kakaru Hashi"",""description"":""Kazuma Hoshino moves to the beautiful countryside to take care of his little brother, Ayumu Hoshino. They ended up  they getting lost on their way to the Japanese Inn."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234113"").data('bubble_data', {""name"":""A Dark Rabbit has Seven Lives"",""description"":""Freshman Kurogane Taito was always an ordinary, regular guy, except for a promise made to the beautiful vampire, Saitohimea nine years ago."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_156063"").data('bubble_data', {""name"":""Abunai Sisters"",""description"":""KOKO & MIKA are actresses by profession: popular and setting out to charm men with their dynamic and sexy bodies\uff0e"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_153359"").data('bubble_data', {""name"":""Adventures in Voice Acting"",""description"":""Volume One is much more than a documentary. It\u2019s a virtual toolkit that contains everything you\u2019ll need to start on your journey into the world of Voice Acting."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47180"").data('bubble_data', {""name"":""Air Master"",""description"":""A former gymnast, Aikawa Maki has turned her skills to a different way of life - street fighting. The only thing that truly makes her feel alive is violence."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_245914"").data('bubble_data', {""name"":""Angel Beats"",""description"":""Enter a description."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_230882"").data('bubble_data', {""name"":""AniView"",""description"":""AniView is a weekly podcast, filmed at the Anime Expo headquarters in California."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240790"").data('bubble_data', {""name"":""Ano Natsu de Matteru"",""description"":""To combat the approaching boredom, friends decide to make a film together documenting their memories together."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_247214"").data('bubble_data', {""name"":""Anohana: The Flower We Saw That Day"",""description"":""Jinta Yadomi and his group of childhood friends have become estranged after a tragic accident split them apart. Now in their high school years, a sudden surprise forces each of them to confront their guilt over what happened that day and come to terms with the ghosts of their past."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240622"").data('bubble_data', {""name"":""Another"",""description"":""The school held a secret that nobody must speak of...Spring, 1998."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_219471"").data('bubble_data', {""name"":""Artichoke and Peachies Show"",""description"":""Two bored girls showcasing anime and Asian culture"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_213938"").data('bubble_data', {""name"":""Asobi ni Ikuyo: Bombshells from the Sky"",""description"":""Kio and all her family members are gathered for a funeral in Okinawa. This same day, a message arrives from outer space. There she encounters a girl named Elis who claims to be an alien."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229044"").data('bubble_data', {""name"":""Astarotte's Toy"",""description"":""Naoya is taken to a magical land where he is to enter succubus Princess Lotte's harem."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_119408"").data('bubble_data', {""name"":""Asura Cryin'"",""description"":""Tomoharu Natsume is a high school student living by himself with a ghost friend that only he can see."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_241656"").data('bubble_data', {""name"":""AX Live"",""description"":""Watch AX Live, broadcasting LIVE every Thursday at 7pm PST at www.anime-expo.org\/ex-online"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229670"").data('bubble_data', {""name"":""Battle Girls - Time Paradox"",""description"":""Modern-day middle school girl Hideyoshi is mysteriously hurled into a world resembling the warring states period of Japanese history, only this time it's inhabited only by women."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_225542"").data('bubble_data', {""name"":""Beelzebub"",""description"":""Oga Tatsumi is a first year student in Ishiyama High, a notorious school for delinquents. One day he sees a man floating by, and the man suddenly splits in half to reveal a baby boy inside!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_52948"").data('bubble_data', {""name"":""Black Jack"",""description"":""Black Jack is an \""unregistered\"" doctor with a clouded, mysterious past."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_197925"").data('bubble_data', {""name"":""Black Jack Motion Magazine"",""description"":""Tezuka Osamu Motion Magazine \nTezuka Osamu Motion Magazine is the evolution of manga from something that you read, to something that you can watch!  It's an entirely new form of manga, presenting to you Tezuka's original works."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_62210"").data('bubble_data', {""name"":""BLASSREITER"",""description"":""The story takes place in a city in Germany sometime in the near future.  A puzzling incident occurs where a corpse rises up in the form of a strange creature and attacks people."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_42854"").data('bubble_data', {""name"":""Bleach"",""description"":""BLEACH follows the story of Ichigo Kurosaki. When Ichigo meets Rukia he finds his life is changed forever."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_247566"").data('bubble_data', {""name"":""Blue Drop"",""description"":""Five years ago, something horrifying happened on Kamioki Island. Something so nightmarish that it stripped all memory from Mari Wakatake's mind even as it left every other human on the island dead in its wake!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_226293"").data('bubble_data', {""name"":""Blue Exorcist"",""description"":""Assiah, the realm of humans, and Gehenna, the realm of demons. Normally, these two dimensions would never intersect, but  the demons are now intruding on the material world."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240614"").data('bubble_data', {""name"":""Bodacious Space Pirates"",""description"":""Marika Katou is a freshman in high school who lives on the planet \""Umi no Akehoshi\"" somewhere in space.  One day, she is told that her father, who she thought was dead, was actually alive until just a while ago, and was the captain of the space pirate ship Bentenmaru.  And she also finds out that they need her, his heir, to be captain so that they can keep flying!  Marika begins a new life as a high school girl, and as captain of the pirate ship Bentenmaru!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240618"").data('bubble_data', {""name"":""Brave 10"",""description"":""Before the Warring States era came to a close, legend has it that Yukimura Sanada has been gathering ten warriors known as Sanada's Brave 10, who have the power to change history."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_230548"").data('bubble_data', {""name"":""Break Ups"",""description"":""'Break Ups' is a slice of life story about a young couple with an on-and-off relationship. They stumble upon a time machine that takes them back to several periods in their lives as a couple."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246842"").data('bubble_data', {""name"":""Campione!"",""description"":""CAMPIONE! surrounds protagonist Godo Kusanagi, a high school student that kills a god, claiming its power and title of Campione, to slay other gods. This harem love comedy is bound to be filled with magical battles."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_221953"").data('bubble_data', {""name"":""Canvas 2: Niji Iro no Sketch"",""description"":""Hiroki Kamikura is a main character of this story that\u2019s a student of Nadeshiko University and serves as an adviser for Art Department in Nadeshiko High School."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_49950"").data('bubble_data', {""name"":""Captain Harlock"",""description"":""The year is 2977. Mankind has become complacent and stagnant. All work is done by machines, while humans spend all their time on entertainment."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_224833"").data('bubble_data', {""name"":""Cardfight Vanguard (Season 1)"",""description"":""Cardfight!! Vanguard Asia Circuit (Season 2) - http:\/\/www.crunchyroll.com\/vanguard2 \n \nSendo Aichi, is a timid boy in his third year of middle school. He had been living his life looking backward. However, he had one thing that kept him going - the \""Blaster Blade.\"""",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_248390"").data('bubble_data', {""name"":""Cardfight!! Vanguard Asia Circuit (Season 2)"",""description"":""Cardfight Vanguard (Season 1) - http:\/\/www.crunchyroll.com\/vanguard1 \n \nThe card game \u2018Vanguard\u2019 has swept the entire world and changed the fate of one boy. His name is Aichi Sendou.  To make an impression on Toshiki Kai, a Cardfighter he deeply admires, Aichi begins to play Vanguard and soon becomes caught up in the fun and charm of the game.Through playing Vanguard, Aichi first meets his teammates-to-be, Misaki Tokura and Kamui Katsuragi, then encounters a great number of allies and rivals, his bond to them strengthening as time goes by. At one point, though, the peculiar power known as Psyqualia awakens within Aichi. It\u2019s the ultimate power that allows Aichi to be in control of his fights by putting him in synch with the planet Cray\u2026 However, the power is too strong and Aichi becomes addicted to it, thereby losing himself. \t \n \nAichi is saved, though, by Kai, Kourin, and the other friends he\u2019s made through playing Vanguard. Aichi gains a variety of experiences and grows tremendously as both a person and Cardfighter. At the national tournament, Aichi\u2019s team defeats AL4, the team led by Ren Suzugamori, Japan\u2019s strongest fighter, and wins the championship.\t \n \nOne day not long afterwards, Aichi and his teammates receive an invitation to participate in the Vanguard Fight Circuit. They\u2019ve broken through the shell that is Japan and want to fly through Asia, where the world\u2019s most powerful teams are waiting for them!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234147"").data('bubble_data', {""name"":""Cat God"",""description"":""Koyama Yuzu is running an antique shop. Mayu, the nekogami (Cat God), is living off Yuzu and leads an idle life playing games."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_100430"").data('bubble_data', {""name"":""Catblue Dynamite"",""description"":""Set in a stylized version of the 1970s, CATBLUE DYNAMITE is an action adventure about Blue (Anna Kunnecke), a principled mercenary who is half-human, half-cat and all female."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_89607"").data('bubble_data', {""name"":""Chance Pop Session"",""description"":""Fascinated by music...Three sisters separated from their mother since they were children..The dramatic memories of people who are fascinated by music..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_179866"").data('bubble_data', {""name"":""Charger Girl Ju-den Chan"",""description"":""Sendo is just an ordinary college student, until the girl Plug appears before him."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_62162"").data('bubble_data', {""name"":""Chi's Sweet Home - Chi's New Address"",""description"":""A heart-warming story of a kitten's daily adventures and its owner family."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_237918"").data('bubble_data', {""name"":""Chihayafuru"",""description"":""Chihaya Ayase  has spent most of her life supporting her sister\u2019s model career. When she meets a boy named Arata Wataya, he thinks Chihaya has potential to become a great karuta player. As Chihaya dreams of becoming Japan's best karuta player, she is soon separated from her karuta playing friends. Now in high school, Chihaya still plays karuta in the hope that she will one day meet her friends again."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246432"").data('bubble_data', {""name"":""Chitose Get You!"",""description"":""Chitose Sakuraba is an energetic 11-year-old girl, while her older brother works for the town hall next to the school.  Without regard to her surroundings, she rushes to the office to spend all the time with her her brother whenever she has free time, causing all sorts of trouble. Chitose, her close friends, homeroom teacher, and even her older brother's superiors will get pulled into this slapstick, comedic adventure that you won't want to miss!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_199616"").data('bubble_data', {""name"":""Chu-Bra"",""description"":""Nayu is a twelve-year old girl who enters a private school at the top of her class."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_199629"").data('bubble_data', {""name"":""Cobra the Animation"",""description"":""Cobra is very known space pirate, but decides to change his face and to clear all his memories."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_161434"").data('bubble_data', {""name"":""CODE GEASS Lelouch of the Rebellion"",""description"":""The Holy Empire of Britannia conquered the country previously known as \""Japan\"" and now known just as \""\""Area 11.\""\"""",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_48046"").data('bubble_data', {""name"":""Cosplay Complex OVA"",""description"":""Chako Hasegawa is a bright and mischievous girl who is in her second year of junior high school."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_230428"").data('bubble_data', {""name"":""Culture Japan"",""description"":""Culture Japan is the TV show that brings Japanese Pop Culture to the world! Tis broadcast on Tokyo MX TV in Japan and across Asia on the Animax Network."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_178193"").data('bubble_data', {""name"":""Dark Side Cat"",""description"":""The mysterious stray cat, \""Dark Side Cat\"" dashes through the night city.  Wrapping himself in punk clothing, he loves to play pranks on the humans he hates."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_48004"").data('bubble_data', {""name"":""Dear Boys"",""description"":""Aikawa Kazuhiko, an easy-going and smiling transfer student, tries to rebuild an ailing Mizuho High School basketball club back to its lost fame."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_227658"").data('bubble_data', {""name"":""DEMIAN"",""description"":""A story about an elite government team that pilots a five into one super robot fighting force. They are called to take down their robot\u2019s creator who is now a terrorist."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_206449"").data('bubble_data', {""name"":""Demon King Daimao"",""description"":""Demon King Daimao  follows Akuto Sai as the lead character, who on the day he enters Constant Magic Academy, receives a very unexpected future occupation aptitude test result: \u201cDevil King.\u201d"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_222052"").data('bubble_data', {""name"":""Demonbane"",""description"":""Ark ham City is being terrified by the dreadful crime organization called the Black Lodge.  The War between Demonbane and Masterterion has just begun!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_48048"").data('bubble_data', {""name"":""Digimon Adventure 02"",""description"":""3 years after the adventure of Tai and his friends, a new enemy Digimon Kaiser appears in the Digital World and he is out to control."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_230332"").data('bubble_data', {""name"":""Digimon Tamers"",""description"":""In West Shinjuku the destinies of three Digi-Destined, Takato Matsuki, Henry Wong, and Rika Nonaka  it will be up to them to protect the city from the fierceness of the mad Digimons."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_238724"").data('bubble_data', {""name"":""Digimon Xros Wars - The Young Hunters Who Leapt Through Time"",""description"":""Taiki Kudo and his friends are brought into the Digital World where they combat evil to save the Digital World from being conquered."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_247112"").data('bubble_data', {""name"":""Dog Days"",""description"":""Following Cinque\u2019s wild adventure in the Flonyard \u2013 an alternate world where its inhabitants look like humans but with animal ears \u2013 he is summoned back once again! However, this time he is joined with childhood friend Rebecca and cousin Nanami to become Galette's Hero as the war wages on the battlefield and in desperate need for an end to the fighting\u2026\u201d"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_197931"").data('bubble_data', {""name"":""Dororo Motion Magazine"",""description"":""Tezuka Osamu Motion Magazine \nTezuka Osamu Motion Magazine is the evolution of manga from something that you read, to something that you can watch!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_224841"").data('bubble_data', {""name"":""Dragon Crisis"",""description"":""\""I love you, Ryuji!\"" Boy meets girl, along with love and war!  Now begins the story of Rose and Ryuji!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_199620"").data('bubble_data', {""name"":""Durarara"",""description"":""Ikebukuro, Tokyo.  \r\nMikado Ryugamine is a young man who yearns for the city like no other. At the invitation of his childhood friend, Masaomi Kida, he moves to Ikebukuro and enters school there.  \r\n \r\nCheck out the official Durarara!! site!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244418"").data('bubble_data', {""name"":""Dusk Maiden of Amnesia"",""description"":""DUSK MAIDEN OF AMNESIA revolves around a first-year high school student, Teiichi Niiya who had just enrolled at Seikyou Private Academy. One day while wandering the hallowed halls, he gets lost in one of the school's old buildings and encounters Yuuko Kanoe, who reveals herself as a ghost with no memories. While investigating her death by looking through the school's seven mysteries, they discover the truth about these ghost stories and help those who are troubled\u2026"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_80532"").data('bubble_data', {""name"":""Egg Man"",""description"":""Egg Man, a serial killer is arrested by Karen, a young women officer."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_119444"").data('bubble_data', {""name"":""Erin"",""description"":""The story is about a young woman who is drawn into a war between kingdoms. She has the ability to command animals, including dragons, as if she was playing a musical instrument."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47018"").data('bubble_data', {""name"":""Eureka Seven"",""description"":""For Renton, every day\u2019s the same.  Nothing happens, nothing changes.   Until one day, a giant mecha called an LFO comes crashing down on him literally."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_46844"").data('bubble_data', {""name"":""Eyeshield 21"",""description"":""Since he was a child, Sena had been bullied into running errands which has given him superb speed which the American football team captain wants as part of \""Eye Shield 21\u2026Lightning Speed\""."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_46806"").data('bubble_data', {""name"":""Fairy Musketeers"",""description"":""Join Little Red Riding Hood, Snow White and Briar Rose as they are appointed to be guardians of a sealed key."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_185954"").data('bubble_data', {""name"":""Fairy Tail"",""description"":""The story follows a teenage girl named Lucy Heartfilla who is determined to join the notorious magical Fairy Tail Guild."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_241208"").data('bubble_data', {""name"":""Familiar of Zero F"",""description"":""FAMILIAR OF ZERO F follows the adventures of second-year student, Louise and Saito at the Tristain Academy of Magic."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_238014"").data('bubble_data', {""name"":""Fate Zero"",""description"":""Taking place 10 years before the events of Fate\/stay night, this series chronicles the events of the Fourth Holy Grail War."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_48052"").data('bubble_data', {""name"":""Fist of the North Star"",""description"":""\""You don't even know you're already dead!!\"" The war has turned the world into a nuclear wasteland.  However, humanity had survived, only to relapse into society where violence dominates."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243998"").data('bubble_data', {""name"":""Folktales from Japan"",""description"":""Like in any culture, Japanese kids grow up listening to the stories repeatedly told by their \r\nparents and grandparents. The boy born from a peach; the princess from the moon who is \r\ndiscovered inside a bamboo; the old man who can make a dead cherry tree blossom, etc. These short stories that teach kids to see both the dark and bright sides of life have passed traditional moral values from generation to generation. \r\n \r\nEach half-hour episode of FOLKTALES FROM JAPAN consists of three self-contained stories, well-known and unknown, with a special focus on heartwarming stories that originate from Tohoku, the northern region heavily touched by the earthquake of 2011. May this program help cheer up earthquake victims and cast a light of hope for them."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_219977"").data('bubble_data', {""name"":""Fortune Arterial"",""description"":""After a childhood full of school transfers, Hasekura Kouhei's parents finally send him to a traditionally-Christian boarding school so he won't have to move with them."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_228632"").data('bubble_data', {""name"":""GA Geijutsuka Art Design Class"",""description"":""GA Geijutsuka Art Design Class is a Japanese seinen yonkoma manga series by Satoko Kiyuzuki. The series was serialized in Heiwa Shuppan's moe four-panel manga magazine Comic Gyutto!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_230334"").data('bubble_data', {""name"":""Gaiking: Legend of Daiku-Maryu"",""description"":""Daiya and his father set out for the sea when they saw a great black flame rise above the water. From the flame arose a giant monster and Daiya\u2019s father and the crew disappeared into the sea."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_46828"").data('bubble_data', {""name"":""Galaxy Express 999"",""description"":""In a distant future, Tetsuro a human boy who wants his body replaced with a robotic one. This is possible, but to do so he has to reach the Immortal Planet onboard the space train Galaxy Express 999."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_179116"").data('bubble_data', {""name"":""Galaxy High School"",""description"":""This series chronicles the lives of Doyle Cleverlobe and Aimee Brightower, who are the first exchange students from Earth to attend an inter-stellar high school on the asteroid, Flutor."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47220"").data('bubble_data', {""name"":""Gankutsuou"",""description"":""In an elegant future Paris, a Count returns to wreak havoc on those that betrayed him."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_239574"").data('bubble_data', {""name"":""gdgd Fairies"",""description"":""Pikuku, Shirushiru, and Korokoro live in the Fairy Forest. They hang out in the forest together and are able to use their magical powers, \""Mental and Time Room\""."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_209908"").data('bubble_data', {""name"":""Giant Killing"",""description"":""Experiencing a long streak of poor performances for several years in the Japanese professional football league, the East Tokyo United (ETU) hires Takeshi Tatsumi as manager to try to break the curse."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47620"").data('bubble_data', {""name"":""Gintama"",""description"":""Gintama is a story of a handyman named Gintoki, a samurai with no respect for rules set by the invaders, who's ready to take any job to survive."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_46672"").data('bubble_data', {""name"":""Girl's High"",""description"":""Eriko and her friends finally make it to an exclusive high school and they're ready to make the most of it!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_48766"").data('bubble_data', {""name"":""Glass Mask"",""description"":""Her father passed away very early and her mother lives and works in a crowded Chinese restaurant. Kitajima Maya, a 13-year old girl, has to carry the burden of making ends meet. However...."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_222428"").data('bubble_data', {""name"":""Go Lion"",""description"":""Go Lion begins in the year 1999, when the planet Altea is subdued and enslaved by the Galra Empire."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_117404"").data('bubble_data', {""name"":""Good Luck! Ninomiya-kun"",""description"":""\u201c Ninomiya-kun\u201d is  a serious and sober high school guy, SHUNGO NINOMIYA. He becomes an unlikely romantic hero when his mysterious older sister RYOKO makes him shack up with his sexy classmate MAYU TSUKIMURA."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_225573"").data('bubble_data', {""name"":""Gosick"",""description"":""GOSICK takes place in 1924 in a small, made-up European country of Sauville. It centers on Kazuya Kujo, the third son of a Japanese Imperial soldier."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_178195"").data('bubble_data', {""name"":""Greathunt"",""description"":""Greathunt is a mysterious amalgamation of heavy Toyama accents, music, and absurdest humor!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_58164"").data('bubble_data', {""name"":""H2O: Footprints in the Sand"",""description"":""Takuma Hirose, an emotionally wounded boy becomes blind after his mother died in his childhood."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246614"").data('bubble_data', {""name"":""Hakuoki Reimeiroku"",""description"":""The third season of Hakuoki continues as Reimeiroku is the prequel and tells the story of the dawning of the Shinsengumi."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_199614"").data('bubble_data', {""name"":""Hanamaru Kindergarten"",""description"":""Welcome to Hanamaru Kindergarten! Here, you will find that everyone is super genki and full of beans, and that every day is fun, fun, fun!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_180899"").data('bubble_data', {""name"":""Hanasakeru Seishonen"",""description"":""Kajika is a 14 year old girl. She is the only daughter of the president of the well-known Burnsworth Company. One day, her father makes an odd proposal to Kajika to find a spouse."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229038"").data('bubble_data', {""name"":""Hanasaku Iroha"",""description"":""Hanasaku Iroha centers around 16-year-old Ohana Matsumae who moves from Tokyo to out in the country to live with her grandmother at an onsen ryokan named Kissuis\u014d."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47130"").data('bubble_data', {""name"":""Happiness!"",""description"":""On the holiday just before the Valentines Day, Kohinata Yuma is called by his bad friend Watarase Jun to buy chocolate with her. Since that day, his fate begins to change."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_52008"").data('bubble_data', {""name"":""Hayate no Gotoku"",""description"":""Hayate is a super-unlucky 16 year-old-boy.  By chance, he rescues Nagi, the well-dressed heiress to a mega-rich family from hoodlums, then he gets hired as a live-in butler."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_245916"").data('bubble_data', {""name"":""Hell Girl: Two Mirrors"",""description"":""Whenever there has been Hell to pay, Ai Enma has been the collector. Whatever damnation you wish on another, she can deliver. At the cost of your own soul, of course. This is why she is known as the Hell Girl. Remorseless and implacable, she is the physical embodiment of revenge. But now, after endless years serving the depraved demands of vengeance obsessed mortals, cracks have begun to form in her once emotionless fa\u2021ade and she must take a journey she never expected, to answer the question she's never dared to ask before: when you're already in Hell, are you allowed to die? The volatile emotions that Ai has kept entombed in her soul have begun to exhume themselves and the answer lies buried in HELL GIRL - TWO MIRRORS - THE COMPLETE COLLECTION!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_206376"").data('bubble_data', {""name"":""HEROMAN"",""description"":""Joey was working hard.  One day he finds an abandoned toy and fixes it. One evening a lightning hits Joey's home.  The electric sparks surround the toy, and Joey witnesses the birth of \""Heroman\""."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243068"").data('bubble_data', {""name"":""Hiiro No Kakera"",""description"":""Hiiro no Kakera is a Japanese visual novel created by Idea Factory directed at the female market known as an otome game. \r\n \r\nThe protagonist is a teenage girl (Tamaki) who revisits a small village, she remembers from her childhood and gets caught up in her family's history and supernatural dangers surrounding it. \r\n \r\nWhile walking along the hillsides waiting for the person who her grandmother sent to fetch Tamaki to the village, Tamaki comes across a small, white round object with sticks for limbs and talks.  Its runs off soon after, with Tamaki chasing after it. Soon Tamaki finds herself in a place where 'it doesn't feel like the world I came from'. She gets attacked by three slime creatures, and a male comes charging in to save her - by clamping his hands around her body and mouth and telling her to be quiet."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_238470"").data('bubble_data', {""name"":""Horizon in the Middle of Nowhere"",""description"":""History is coming to an end. When humans came down from the sky they brought with them the Testament, the guide to the path they must follow if they wish to return to the skies again."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_224830"").data('bubble_data', {""name"":""Hourou Musuko Wandering Son"",""description"":""Nitori Shuichi is a boy who wants to become a girl. He transfers to a new elementary school, and there, meets Takatsuki Yoshino, a tall and attractive young girl."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246860"").data('bubble_data', {""name"":""Humanity Has Declined"",""description"":""Our human race has been slowly declining for several centuries now. In many ways, the Earth already belongs to the Fairies. Life is relaxed and care...free? Thus begins a story that is a little strange and just a tiny bit absurd."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_237800"").data('bubble_data', {""name"":""Hunter x Hunter"",""description"":""Gon, a young boy who lives on Whale Island, dreams of becoming a Hunter like his father, who left when Gon was still young."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_46628"").data('bubble_data', {""name"":""Ikkitousen"",""description"":""When these girls fight - clothes won't last!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_230336"").data('bubble_data', {""name"":""Interlude"",""description"":""High school student, Naoya Aizawa is having a normal day with his long time friend, Tama. But on his way to school, every person in the train station suddenly disappears\u2026.except for one girl."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240612"").data('bubble_data', {""name"":""Inu X Boku Secret Service"",""description"":""In the heavily guarded building known as, \u2018Ayakashi Kan,\u2019 only those who have undergone a strict review can reside there while being accompanied by a Secret Service agent (SS)."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_179178"").data('bubble_data', {""name"":""Japan Tourism Anime Channel"",""description"":""This channel will bring to you anime that helps guide you around famous places to visit in Japan!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_223141"").data('bubble_data', {""name"":""Japancast"",""description"":""Learn about Japanese culture, customs and how to speak Japanese with Japancast.net."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_151168"").data('bubble_data', {""name"":""Japanese Anime Classic Collection 1"",""description"":""Japanese Anime Classic Collection is a set of vintage anime presents 55 titles from the 1920s and 1930s, the Golden Age of Japanese silent film."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_151172"").data('bubble_data', {""name"":""Japanese Anime Classic Collection 3"",""description"":""Japanese Anime Classic Collection is a set of vintage anime presents 55 titles from the 1920s and 1930s, the Golden Age of Japanese silent film."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_151174"").data('bubble_data', {""name"":""Japanese Anime Classic Collection 4"",""description"":""Japanese Anime Classic Collection is a set of vintage anime presents 55 titles from the 1920s and 1930s, the Golden Age of Japanese silent film."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_168263"").data('bubble_data', {""name"":""Kaasan Mom's Life"",""description"":""A sidesplitting essay in animation that depicts the simple everyday life of career-committed mother and her rather out of standard family set in contemporary Japan."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_150701"").data('bubble_data', {""name"":""Kaede New Town"",""description"":""Kaede Newtown takes you back to the good ol' town you lived in during the good ol' days of your childhood."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234149"").data('bubble_data', {""name"":""Kamisama Dolls"",""description"":""KAMISAMA DOLLS follows Kyouhei, a college student. One night, he and Hibino discover a dead body and coincidentally Kyouhei\u2019s sister appears with the doll called Kamisama she controls..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_179836"").data('bubble_data', {""name"":""Kanamemo"",""description"":""Kana Nakamichi lost both her parents which left her homeless.  Kanamemo follows her daily life in a fast-paced yet joyous environment."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_62326"").data('bubble_data', {""name"":""Kanokon"",""description"":""Kanokon's story revolves around Kouta Oyamada, a young first-year high school student who moves from the country to the city and thus transfers to Kunpo High School. On his first day at his new school, a beautiful second-year female student named Chizuru Minamoto asks him to meet her alone in the music room. When he arrives, she reveals her that she is in fact a fox deity and from that day on the two hang out together. Nozomu is a first year female student at Kouta's school, she is in fact a wolf deity and in love with Kouta, and a rival of Chizuru for Kouta's affections."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_118236"").data('bubble_data', {""name"":""Kemeko DX"",""description"":""M.M. pilots the mech known as Kemeko to protect her fianc\u00e9 from the efforts of the elusive Mishima Corporation who is deeply interested in a mysterious power that is hidden within his body."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_87570"").data('bubble_data', {""name"":""Kid Kosmo"",""description"":""Kid Cosmo finds magic gloves! With the magical gloves he saves a princes in trouble!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_185112"").data('bubble_data', {""name"":""Kiddy GiRL-AND"",""description"":""500 years in the future, in Star Year 0379, the bright and cheerful Ascoeur and the slightly more serious Q-feuille work for the galactic government known as GTO."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244298"").data('bubble_data', {""name"":""Kids on the Slope"",""description"":""The story of Kids on the Slope really starts rolling when the classical piano-trained Kaoru encounters Sentaro and falls head-over-heels in love with jazz. One thing to look out for is the performance scene of an excellent jazz piece in every episode. Up-and-coming young jazz musicians are employed here, with Takashi Matsunaga (Kaoru) on piano and Shun Ishiwaka (Sentaro) on drums. Yoko Kanno produces the sessions, which are then used directly for the anime. \r\nJazz standards like \u201cMoanin\u2019\u201d, \u201cMy Favorite Things\u201d, and \u201cSomeday My Prince Will Come\u201d are played along with improvisational performances that reflect the mood of the characters at the time, whether they\u2019re in high spirits, or feel lonely, confused, angry, and so on. How these occasional performances and feelings are portrayed and directed is one more highlight of the show. Director Watanabe\u2019s \u201csessions\u201d are a mix of images and music, so innovative and exciting visual expressions should be expected."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_168118"").data('bubble_data', {""name"":""Kigurumikku"",""description"":""The high spirited girls, put on their  \""KIGURUMI\""costumes instantly transforming them into the super hero's. Follow these super-girls as they defend Minamachi City and bring peace to the town."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_88348"").data('bubble_data', {""name"":""Kite Liberator"",""description"":""Monaka seems to be just an average high school girl. But she has a dark and terrible secret."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_228630"").data('bubble_data', {""name"":""Kobo chan"",""description"":"""",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_81052"").data('bubble_data', {""name"":""Koihime Musou"",""description"":""When Kanu was young, her family and their village were wiped out by bandits.  And so she sets out on a journey. A journey to find the answer."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246836"").data('bubble_data', {""name"":""Kokoro Connect"",""description"":""Five members of the school culture club - Taichi Yaegashi, Iori Nagase, Himeko Inaba, Yui Kiriyama, and Yoshifumi Aoki - encounter a bizarre phenomenon one day when Aoki and Yui switch personalities without warning. The same begins to happen to the other club members, throwing their daily lives into chaos. At first the five students find some amusement among the confusion, but this connection also exposes the painful scars hidden within their hearts... When their calm lives are shattered, the relationships between the five students also begin to change!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_53158"").data('bubble_data', {""name"":""Kono Aozora ni Yakusoku wo"",""description"":""One day, a transfer student moves to town. Though sometimes arguing against each other, the students come to enjoy their slices of life on the island."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_174635"").data('bubble_data', {""name"":""Kurokami The Animation"",""description"":""This is a tale of battle that begins with a chance meeting between a human boy named Keita and a young Tera Guardian girl named Kuro."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243974"").data('bubble_data', {""name"":""Kuroko's Basketball"",""description"":""Kuroko's Basketball \r\n \r\nThe Teiko Middle School Basketball Team. \r\n \r\nThe class that produced three perfect seasons in a row, with five once-in-a generation players, called \""The miracle generation\"". \r\nThere was another player who all of them respected... \r\n \r\nA legendary 6th player. \r\n \r\nAn up-and-coming power player, Taiga Kagami, is just back from America.  When he comes to Seirin High School, he meets the super-ordinary boy, Tetsuya Kuroko. Kagami is shocked to find that Kuroko isn't good at basketball, in fact, he's bad!  And he's so plain that he's impossible to see.  But Kuroko's plainness lets him pass the ball around without the other team noticing him, and he's none other than the sixth member of the Miracle Generation. \r\n \r\nKuroko makes a pact with Kagami to defeat the other members of the Miracle Generation, who have all played basketball at other schools. \r\n \r\nA battle of light (Kagami) and shadow (Kuroko) begins!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47258"").data('bubble_data', {""name"":""La Corda d'Oro ~primo passo~ and ~secondo passo~"",""description"":""Kahoko Hino goes to Seiso Gakuen High School that specializes in music. Having a normal life until one day, Lili the fairy gave her a magical violin."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_119420"").data('bubble_data', {""name"":""La Maison en Petits Cubes"",""description"":""An old man is forced to continuously build on new levels to his home as the water rises in his town.   As he visits the submerged levels of his home, he begins to relive scenes from days gone by."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246838"").data('bubble_data', {""name"":""La storia della Arcana Famiglia"",""description"":""\""I will decide my own path!\"" \r\n \r\nThe prosperous island of Regalo in the Mediterranean Sea is protected by a vigilante group called \""Arcana Famiglia.\"" The group's members are granted special powers through contracts with the \""Toracco.\"" Felicit\u00e1, the only daughter of the Papa and head of the family, has her mother's beauty as well as superior skill with knives. Though she was raised deep in the island, at age 16 she takes her first steps into the spotlight as a member of the family. Then, at her birthday party, her father, Mondo, announces that he is retiring. He declares a competition to determine his successor - and Felicit\u00e1 must marry the winner. Felicit\u00e1 enters the competition herself, determined to win so that she can decide her own path. The curtain will soon rise on the battle that will determine her fate!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_89610"").data('bubble_data', {""name"":""Lady Death"",""description"":""Eve of invasion of D is Night. Lady Death's camp. Her demon hordes blanket the land."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243602"").data('bubble_data', {""name"":""Leiji Matsumoto's OZMA"",""description"":""\u201cLeiji Matsumoto's OZMA \u2013 the show unfolds on an arid and devastated future Earth, and involves the mysterious, giant and moving \u2018\u201cOZMA.\u2019\u201d An epic space opera that entwines exciting sci-fi action and a suspenseful story, anime godfather Leiji Matsumoto tackles the ultimate questions of life and existence.\u201d"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_224791"").data('bubble_data', {""name"":""Level E"",""description"":""Hundreds of aliens inhabit our Earth today. One day, a hell of a troublemaker arrives on our planet. ."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_206590"").data('bubble_data', {""name"":""Lilpri"",""description"":""The queen of the Wonderland orders her \u201cMa(gic)Pets\u201d to find three human girls who have the potential to become the Princesses in order to save Wonderland which is on the verge of extinction."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_96409"").data('bubble_data', {""name"":""Linebarrels of Iron"",""description"":""Fourteen-year-old Kouichi Hayase\u2019s life has always been a mediocre one, if not dismal. However, those days of being bullied by classmates and escaping to a fantasy of being a hero are put to an end when a certain \u201caccident\u201d bestows on him a girl and a gigantic humanoid robot called \u201cLINEBARREL\u201d."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240616"").data('bubble_data', {""name"":""Listen to Me, Girls, I\u2019m Your Father!"",""description"":""Yuuta Segawa, a college student, suddenly becomes the guardian of his three nieces in place of his sister and her husband. The eldest niece, Sora, is a 14 year old in middle school with semi-long brownish hair decorated with a ribbon. The middle niece, Miu, is an elementary schoolgirl with blond pigtails. The youngest niece, Hina, is an angelic 3 year old attending daycare. This at-home romantic comedy illustrates the chaotic but heartwarming lives of these four characters in their tiny one-room apartment."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_239534"").data('bubble_data', {""name"":""Little Nemo: Adventures in Slumberland"",""description"":""Welcome to the fantasy world of \""Little Nemo,\"" filled with dreams of enchanted lands and new friends, amazing magic and fun-filled adventure. A place where anything is possible and the only boundaries are those of the imagination. In this major motion picture, Nemo journeys to the Kingdom of Slumberland. The King of Slumberland welcomes Nemo with open arts, making him heir to the throne and giving him a magical key that opens any door in the kingdom. \""But I must warn you,\"" the King says, \""there is one door you must never open.\"" Not heeding the King's advice, Nemo unlocks the door.  \n \nWith the King kidnapped and the nightmare unleashed upon the kind people of Slumberland, Nemo and his friends must venture into the depths of the Nightmare World in a courageous attempt to make things right. Will they be able to save the King and restore peace to the Kingdom of Slumberland? Only then will Nemo dream happily ever after."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_52148"").data('bubble_data', {""name"":""Lucky Star"",""description"":""Though Konata Izumi, a high school girl, is extremely athletic, she refuses to join any of the school\u2019s sports teams."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47712"").data('bubble_data', {""name"":""Maburaho"",""description"":""Maburaho is set in a world where every character has the ability to use magic, however everyone's magic is not equal."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_89618"").data('bubble_data', {""name"":""Magical Play"",""description"":""Little Padudu is a girl trying to become a renowned magical warrior."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_222071"").data('bubble_data', {""name"":""Magicians Academy"",""description"":""Magician's Academy revolves around Takuto Hasegawa, who attends a magic academy."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_238020"").data('bubble_data', {""name"":""Majikoi Oh! Samurai Girls"",""description"":""A fierce battle takes place in the mountains of Kanagawa between students of Kawakami Academy, a school that resolves internal disputes with martial arts!  Behind the scenes, we see a love story unfold between a brilliant tactician and the strongest of the Big Four, a girl with the nickname \""the God of Martial Arts.\"" But it seems there are others after the young tactician's heart.  Watch to find out just what class wins the battle and what happens when the other three members of the Big Four show up..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_238580"").data('bubble_data', {""name"":""Mashiroiro Symphony"",""description"":""When the decision is made to merge the elite Yuihime Girls' Private Academy and Kagamidai Boys Private Academy, everyone wants to take extra care in avoiding trouble while bringing the two together."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244228"").data('bubble_data', {""name"":""Medaka Box"",""description"":""The gorgeous, peerless student council president, Medaka Kurokami, solves everyone's problems! Medaka Kurokami, who has just become Sandbox Academy's 98th student council president, sets up a suggestion box as promised in her election campaign. \""Your dreams belong to you. Go after them and make them come true yourself. Your worries, however, belong to me. Send them all my way!!\"" As Medaka solves her fellow students' problems along with her friend since childhood, Zenkichi Hitoyoshi, her suggestion box, nicknamed the Medaka Box, earns great popularity among the students."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_87578"").data('bubble_data', {""name"":""Mighty Max"",""description"":""A girl gets powers from mighty max and saves the day! Or, does she?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_192183"").data('bubble_data', {""name"":""Miracle Train"",""description"":""There is an urban legend in Japan about an underground subway line that moves without limitations. The rumor is only gorgeous men ride a train that is known as the Miracle Train."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_214727"").data('bubble_data', {""name"":""Mitsudomoe"",""description"":""The three Marui sisters, Mitsuba, Futaba, and Hitoha are not your typical Japanese students. Satoshi Yabe who's a newly hired teacher will have to learn to deal with these three girls as they terroize him or it will end up getting the best of him. Will he survive the Marui triplets?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_53762"").data('bubble_data', {""name"":""Mizu no Kotoba"",""description"":""A certain caf\u00e9. Seven men and women, drawn together by chance. Their conversations interweave to create a mysterious interval of time. A world protected by Kotonoha, a space where fluid words have a life of their own. Go ahead, take a look. \n \nDon't miss other works by director Yasuhiro YOSHIURA here on Crunchyroll! The \""Time of Eve\"" group features video and text interviews with Yasuhiro Yoshiura!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_200143"").data('bubble_data', {""name"":""Mobile Suit Gundam"",""description"":""U.C. 0079. The rebel space colonies of the Principality of Zeon launch a war of independence against the Earth Federation, using humanoid fighting vehicles called mobile suits to overwhelm the Federation Forces and conquer half of Earth's surface. \r\n \r\nMonths later, the Federation has finally developed its own prototype mobile suits at a remote space colony. But when the colony suffers a Zeon surprise attack, these new weapons fall into the hands of a motley crew of civilians and cadets, and fate places a youth named Amuro Ray at the controls of the white mobile suit Gundam..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_172756"").data('bubble_data', {""name"":""MOBILE SUIT GUNDAM 00"",""description"":""2307 A.D. - Humanity has obtained a new source of energy to replace fossil fuels: large-scale solar power generation system based on three huge orbital elevators. However, the benefits are available only to a handful of major powers and their allies. A private armed organization appears, dedicated to the elimination of war through armed force. Its name is Celestial Being, and it is in possession of \""Gundam\"" mobile suits. With these Gundams, it begins armed intervention into all acts of war."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_200853"").data('bubble_data', {""name"":""Mobile Suit Gundam UC (Unicorn)"",""description"":""The year is U.C. 0096. Three years have passed since the end of the Second Neo Zeon War. \r\n \r\nIt is said that the Vist Foundation manipulates the Earth Federation and Anaheim Electronics from behind the scenes.  Hoping to create a new world, the Foundation attempts to hand over a certain secret to the Neo Zeon remnants known as the Sleeves.  This will mean the opening of Laplace's Box, which holds a great secret tied to the origins of the Universal Century. \r\n \r\nThe exchange between the Vist Foundation and the Sleeves is to take place at the manufacturing colony Industrial 7.  This is the home of the student Banagher Links, who rescues a girl he sees falling through the colony's zero gravity area.  The girl gives her name as Audrey Burne and says she wants to prevent a war, spurring Banagher to step into the conflict surrounding Laplace's Box\u2014almost as if he is drawn in by his own bloodline. \r\n \r\nBased on a story by author Harutoshi Fukui, the newest Gundam work dynamically unfolds against the backdrop of the Universal Century. It all begins with this first shocking episode."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_115438"").data('bubble_data', {""name"":""Mobile Suit Gundam Wing"",""description"":""Centuries in the future, in the year After Colony 195, Earth is surrounded by orbiting space colonies. The colonists are cruelly oppressed by the Earth Alliance, which uses huge humanoid fighting machines called \""mobile suits\"" to control the populace. Behind this tyranny is the secret society called \""Oz,\"" which has infiltrated the Alliance military and steered it towards its repressive course."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_115432"").data('bubble_data', {""name"":""Mobile Suit Zeta Gundam"",""description"":""U.C. 0087. Seven years after the end of the One Year War, the people of the space colonies continue to demand freedom and justice from the corrupt Earth Federation - demands that the Federation's elite Titans taskforce suppresses with cruel violence. Now a rebel force called the AEUG is about to strike back against the tyranny of the Titans. \r\n  When the AEUG launches an attack on the Titan headquarters, the political conflict escalates into a civil war. And a young civilian named Kamille Bidan, swept up in the conflict, becomes the Gundam pilot of this new era..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_225580"").data('bubble_data', {""name"":""Moribito"",""description"":""She carries the pain of eight souls. He carries the burden of one sacred spirit. At a time when the balance of nature still held the civilizations of mankind in thrall, a single drought could spell the end of a society and doom its inhabitants to piteous deaths. Prince Chagum has been imbued with the power to stave off the drought and bring new life to his empire. However, this is a suspicious time, and he is accused of possession by an evil spirit. Court advisors only see one solution. Chagum must be put to death by his own father's hand. His salvation is in the form of Balsa, a spear woman and mercenary from Kanbal, the kingdom across the mountains. Her skills are legendary, and although reluctant, she is held by a mysterious vow to save eight souls before she dies. Can she fend off an entire empire and make Chagum her eighth soul?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_235340"").data('bubble_data', {""name"":""Moritasan wa Mukuchi"",""description"":""A short original video animation (OVA) directed by Naotaka Hayashi and produced by Studio Gram was bundled as a DVD with the limited edition of the third manga volume sold on February 26, 2011. Another OVA DVD, produced by the same staff as the previous OVA, was released separately on March 25, 2011. A TV anime series, with the same staff as the OVAs, began airing in Japan in July 2011. \r\n \r\nThe second OVA features two pieces of theme music: one opening theme and one ending theme. The opening theme is \""Morita-san wa Mukuchi\"" (\u68ee\u7530\u3055\u3093\u306f\u7121\u53e3?) sung by Kana Hanazawa and Haruka Tomatsu, and the ending theme is \""T\u014dmawarishite Kaero!\"" (\u9060\u56de\u308a\u3057\u3066\u5e30\u308d!?) sung by Yoshino Nanj\u014d and Saori Hayami. A single containing the theme songs was released on March 25, 2011."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47486"").data('bubble_data', {""name"":""Mouse"",""description"":""\u30de\u30a6\u30b9 \r\n \r\nFor centuries, there was a family of thieves stealing money and priceless property. In the latest version, a college student is surrounded by three attractive assistants, who help him pull of heists of art museums and landmark towers. The thieves had the power to steal entire buildings and take structures out to sea but never get caught."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_247118"").data('bubble_data', {""name"":""Moyashimon"",""description"":""MOYASHIMON RETURNS picks up where the last season left off as our protagonist Tadayasu Sawaki, a first-year college student at an agricultural university, continues to have the unique ability to see and communicate with micro-organisms and bacteria. Still alongside good friend Kei Yuuki, whose family runs a sake brewery, he devolves an even deeper understanding of bacteria world with his special ability!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_50538"").data('bubble_data', {""name"":""Mushi-Uta"",""description"":""Teenagers who live with \u201cdreams\u201d in their hearts.  \r\n \r\nWhen their dreams become so big and powerful that they cannot suppress them anymore, the \u201cbugs\u201d (supernatural beings that look like bugs) appear out of nowhere and eat their dreams. The bugs deprive teenagers of their dreams, but give them unwanted superhuman power.  \r\n \r\nThe bugs are parasites to humans and their hosts are called the \u201cMushi-Tsuki.\u201d Though the Mushi-Tsuki\u2019s existence is officially denied, everybody knows the term and the Mushi-Tsuki are discriminated against and feared by the public.  \r\n \r\nThe near future, a futuristic Japan. It is now ten years after the \u201cbugs\u201d first appeared.  \r\n \r\n\u201cSpecial Environment Preservation Office\u201d or \u201cSEP\u201d is an entity attached to a governmental organization.  The government\u2019s official announcement declares its mission is to find and catch the Mushi-Tsuki, and render them \u201cnon-existent.\u201d  \u201cCuckoo\u201d is SEP\u2019s best agent, and the strongest Mushi-Tsuki. He is possessed by a green \u201ccuckoo bug.\u201d He can draw the bug\u2019s power into himself and his guns, increasing both his physical abilities and those of his weapons.  One day, Cuckoo is assigned to catch a girl, Shiika Anmoto.  She ran away from a secret facility where the Mushi-Tsuki\u3000are locked away.  \r\n \r\nDaisuke Kusuriya, who looks completely ordinary, is a freshman at Oka Higashi High School. He meets Shiika Anmoto on his way to school and they are strongly attracted to one another.  Meanwhile, Cuckoo tries to corner Shiika Anmoto, but members of the \u201cMushi-Ban\u00ea \u201d appear. Mushi-Ban\u00ea is the resistance organization against SEP, led by Ladybird.  \r\n \r\nThe sacred night\u2019s battle, which are bursting with the poignant and passionate emotions of the dreamers.has just began."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229046"").data('bubble_data', {""name"":""My Ordinary Life"",""description"":""If you think this is going to be a typical story of school life, you'll be surprised to know that the ordinary definitely doesn't happen in My Ordinary Life."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_55898"").data('bubble_data', {""name"":""Myself; Yourself"",""description"":""Sana Hikada returns to his hometown after 5 years away living in Tokyo. Although there are some changes to the town, most has remained the same. He notices a girl in a shrine maiden outfit, Nanaka Yatsushiro, who is a old childhood friend whom he gave a bracelet to before he left which she still wears to this day."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244056"").data('bubble_data', {""name"":""Mysterious Girlfriend X"",""description"":""Tsubaki Akira, a completely ordinary boy in high school, happens to lick the saliva of a mysterious girl, Urabe Mikoto, who just transferred into his class. The next day, Tsubaki is bed-ridden with a inexplicable fever that won't break. Five days later, Urabe makes an unexpected visit to Tsubaki's room and tells him to lick her saliva, after which his fever immediately breaks. Tsubaki asks Urabe what caused the fever, and she responds that he was lovesick. That marks the beginning of Tsubaki and Urabe's very ordinary yet somewhat odd romance!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_247344"").data('bubble_data', {""name"":""Nakaimo - My Sister is Among Them!"",""description"":""In accordance with his father's will, Shougo Mikadono transfers to the school that his future wife is said to be attending. But also attending the school is Shougo's younger sister who was separated at birth, and he doesn't know what she looks like - but she attempts to get close to him without revealing who she is! Can Shougo figure out which of the girls at this school is his sister and find his destined life partner at the same time?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_189368"").data('bubble_data', {""name"":""Naked Wolves"",""description"":""\u201cI am gonna be a sumo wrestler as grand as Mt. Fuji~!\u201d \nIn the comedic adventures of \u201cNaked Wolves\u201d, the central character, a sumo wrestler named Mikoshiarashi, travels around the world acquiring top secret sumo techniques and making friends wiith strong wrestlers on his road to becoming an invincible \u201cYOKOZUNA\u201d \u2013 The Grand Sumo Champion.  Howl on, Naked Wolves!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_42850"").data('bubble_data', {""name"":""Naruto"",""description"":""The Village Hidden in the Leaves is home to the stealthiest ninja.  But twelve years earlier, a fearsome Nine-tailed Fox terrorized the village before it was subdued and its spirit sealed within the body of a baby boy."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_42852"").data('bubble_data', {""name"":""Naruto Shippuden"",""description"":""Naruto Uzumaki wants to be the best ninja in the land. He's done well so far, but with the looming danger posed by the mysterious Akatsuki organization, Naruto knows he must train harder than ever and leaves his village for intense exercises that will push him to his limits."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244046"").data('bubble_data', {""name"":""NARUTO Spin-Off: Rock Lee & His Ninja Pals"",""description"":""Welcome to the Hidden Leaf Village. The village where Uzumaki Naruto, star of the TV show \""Naruto\"" makes his home. Every day, countless powerful ninjas carry out missions and train to hone their skills. Our main character is one of these powerful ninjas...but it's not Naruto! It's the ninja who can't use ninjutsu, Rock Lee! \r\n \r\nIn spite of his handicap, Lee has big dreams. He works hard every day to perfect his hand-to-hand combat skills and become a splendid ninja!  And to achieve his dream, he puts in more effort than anyone else. Under the hot-blooded tutelage of his teacher Guy, \r\nhe works alongside his teammates Tenten and Neji. Watch the Beautiful Green Beast Rock Lee train, go on missions, and have all sorts of adventures!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_169063"").data('bubble_data', {""name"":""NATSU NO ARASHI !"",""description"":""At the old town's coffee shop, Yasaka - in a chance encounter - meets an older high school student, named Arashi. Just his luck, Yasaka finds out she is in fact a ghost, as the two take off on an unusual journey together! The events that transpired at the coffee house will kick off an unforgettable summer holiday with an amazing story that is guaranteed to captivate audiences of all types!~"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_81875"").data('bubble_data', {""name"":""Natsume Yujin-cho"",""description"":""Natsume Takashi has the ability to see spirits, which he has long kept secret. However, once he inherits a strange book that belonged to his deceased grandmother, Reiko, he discovers the reason why spirits surround him."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_241846"").data('bubble_data', {""name"":""Natsume Yujin-cho Shi"",""description"":""Takashi Natsume has been able to see youkai, Japanese monsters, his whole life.  He inherited a book of youkai names from his grandmother, Reiko.  These names give their owner power over the youkai, so he and his self-proclaimed bodyguard \""Nyanko-sensei\"" spend their days returning them to their original owners.  As Natsume finally finds a place where he feels welcome, he meets and says goodbye to new Youkai...."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246682"").data('bubble_data', {""name"":""Natsuyuki Rendezvous"",""description"":""Ryosuke works part-time as a florist where he begins to fall for the owner, Rokka. Eight years ago, Rokka decided to give up on love but one day, due to circumstances, Ryosuke ends up in Rokka's apartment where he runs into a half-naked man. Having mixed emotions, he realizes that this man is her late husband's ghost. However, Rokka is unable to see him. Will Ryosuke be able to pursue his love despite the presence of her late husband?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_62522"").data('bubble_data', {""name"":""Neo Angelique Abyss"",""description"":""The story takes place in a fictional world called Arcadia, where life-draining monsters called Thanatos plague the populace. The only ones who have the power to exterminate these creatures are Purifiers, but only a few exist. One day, Angelique, who is just a presumably normal girl attending school, is visited by Nyx, a rich gentlemen, as well as a Purifier, who created an organization comprised of Purifiers dedicated to eliminating Thanatos."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_245920"").data('bubble_data', {""name"":""Night Raid 1931"",""description"":""The year is 1931. The City is Shanghai. Ten years before America will enter World War II; the hydra's teeth planted by the first great global conflict are beginning to germinate, hatching into spiders weaving the complex web of plots and conspiracies destined to inevitably draw entire nations, against their will, to the brink of destruction once more. And caught in the heart of these webs, desperately seeking to separate lies from truth, is \""Sakurai Kikan,\"" an ultra-secret intelligence agency staffed by extraordinarily talented individuals with abilities far beyond those of normal humans. Their duty: stop the darkest plots and eliminate the greatest threats.  \r\n \r\nBut in a city built on intrigue, can even a team of clairvoyants, telepaths and espers stand against the ultimate forces of destiny? As the Japanese Imperial Army rolls across China, documented facts and fantasy intermingle in the breathtaking exploration of alternative history that is Night Raid 1931."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240610"").data('bubble_data', {""name"":""Nisemonogatari"",""description"":""In Bakemonogatari, the story centers on Koyomi Araragi, a third year high school student who has recently survived a vampire attack, and finds himself mixed up with all kinds of apparitions: gods, ghosts, myths, and spirits. However, in Nisemonogatari, we pick up right where we left off and follow Koyomi as the psychological twists delve deeper and deeper\u2026"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234107"").data('bubble_data', {""name"":""No. 6"",""description"":""Sion is a bright teenager living a comfortable and promising life inside No. 6, one of this six remaining city-states created by The Babylon Treaty after the last great war devastated the world. On the rainy evening of his twelfth birthday, he meets a savvy adolescent who calls himself \""Nezumi\"" (Rat) and is desperately trying to runaway from the authorities. For helping a fugitive of the state, Sion is stripped of all his privileges. Four years later, they meet once again. For better or for worse, Sion is about to unravel the secrets guarded deep inside No. 6. \r\n \r\nSource: ANN"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_241436"").data('bubble_data', {""name"":""Nyarko-san: Another Crawling Chaos"",""description"":""\""I'm the chaos that always crawls up to you with a smile, Nyarlathotep.\"" \nA silver haired girl appeared with this nonsensical catch phrase! \n \nShe is the malign deity \""Crawling Chaos\"" Nyarlathotep, or Nyarko for short. \nTogether with \""Living Flame\"" Cthuga, or Kuko, and \""The Unspeakable One\"" Hastur, or Hasuta, they bring you an abysmally terrifying Love(craft) comedy! \n \nDeities arrive on Earth one after another in search of Yasaka Mahiro and Nyaruko. \nNyaruko and her Space CQC take on all of them. \nWhat is the truth behind these incidents of cosmic scale? \nWill Mahiro be able to live in peace? \n \nThis tumultuous and chaotic comedy is crawling to a TV near you!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_212723"").data('bubble_data', {""name"":""Occult Academy"",""description"":""The year is 1999. At the summit of Japan\u2019s \u201cpyramid\u201d, Minakamiyama, stands Waldstein academy. Strange occult phenomena occur here, as if drawn by some mysterious force.  Those who know it call it \u201cOccult Academy\u201d."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_199612"").data('bubble_data', {""name"":""Omamori Himari"",""description"":""Yuuto Amakawa comes from a long line of demon-slayers. Orphaned seven years ago without a single living relative, he has been protected and been able to live life as an ordinary high school boy \u2013 until his sixteenth birthday when his protection charm wears off. Just as he is about to be in real big, demon-packed trouble, a mysterious catgirl named Himari appears and pledges her allegiance to him, promising to protect him forever."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_236074"").data('bubble_data', {""name"":""Oreimo"",""description"":""Kyosuke Kosaka, a normal 17-year-old high school student living in Chiba, has not gotten along with his younger sister Kirino in years. For longer than he can remember, Kirino has ignored his comings and goings and looked at him with spurning eyes. It seemed as if the relationship between Ky\u014dsuke and his sister, now fourteen, would continue this way forever. One day however, Kyosuke finds a DVD case of a magical girl anime which had fallen in his house's entrance way. To Kyosuke's surprise, he finds a hidden eroge (an adult game) inside the case and he soon learns that both the DVD and the game belong to Kirino. That night, Kirino brings Kyosuke to her room and reveals herself to be an otaku with an extensive collection of moe anime and younger sister-themed eroge she has been collecting in secret. Kyosuke quickly becomes Kirino's confidant for her secret hobby."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_226838"").data('bubble_data', {""name"":""Otaku No Video Podcast"",""description"":""A series of spoiler-free anime reviews, going beyond \""I liked it\"" or \""I hated it\"" and telling you about what a series actually is. I go deeper."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_223212"").data('bubble_data', {""name"":""Otaku-Verse Zero"",""description"":""Otaku-Verse Zero aims to bring \u201cotaku all over the universe\u201d fun and lively coverage of anime, manga, and Japanese pop culture."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_220642"").data('bubble_data', {""name"":""Otome Yokai Zakuro"",""description"":""\""Demon Girl Zakuro,\"" an ongoing favorite in Gentosha Comics's Monthly \""Comic Birz,\"" is being adapted into anime form!  Watch Lily Hoshino's gorgeous characters in motion!  \n \nThis story takes place during the Westernization Movement in an alternate world where humans and spirits coexist as young half-spirit girls team up with army officers to battle against spirits who would do wrong! \n \nOnward with the spirits!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_48994"").data('bubble_data', {""name"":""Pale Cocoon"",""description"":""Before we knew it, humans were living here in this world. A future where the continuity of history has broken off, a world of enormous ruins that continue endlessly. Ura works in the Archive Excavation Dept., which excavates records of the past world. One day, he restores a mysterious visual record... \n \nDon't miss other works by Yasuhiro YOSHIURA on Crunchyroll! The Time of Eve group features video and text interviews with Yoshiura! \n \n http:\/\/www.crunchyroll.com\/library\/Time_of_Eve  \n http:\/\/www.crunchyroll.com\/library\/Mizu_no_Kotoba  \n \nOfficial website (Japanese only): \n http:\/\/www.studio-rikka.com\/page\/pale\/pale_top.htm  \n \nProduction information: \n2005 \napprox. 23 minutes \n \nStory\/script\/production: Yasuhiro YOSHIURA \nMusic: Toru OKADA \nSound effects: Kazumi \u014cKUBO \nProducer: Tom NAGAE \n \nDirector YOSHIURA on the production style: \n\"" I created the characters using hand-drawn animation (pencil on drawing paper), and the backgrounds are a blend of hand-drawn animation and 3DCG. Rather than placing 2D drawings over 3D backgrounds, I aimed to use both styles to create a unified visual image.\"""",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_89628"").data('bubble_data', {""name"":""Parasite Dolls"",""description"":""Branch specializes in crimes involving humanoid robots called 'Boomers'. Branch officers Buzz and Michaelson protect a world that is slowly deconstructing around them."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_167129"").data('bubble_data', {""name"":""Peeping Life"",""description"":""If you're a fan of shows like Comedy Central's \""Shorties Watching Shorties\"" and independent style animation, with a sharp sense of humor, this series is for you! \n \nBrought to you by CoMixWaveFilms, Peeping life presents hilarious shorts of animated rodoscoped skits by popular Japanese comic-duos. The Peeping Life series is currently airing on Japanese TV and Cruchyroll to rest of the world."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_232188"").data('bubble_data', {""name"":""Persona"",""description"":""Purchase the PERSONA -Trinity Soul- DVD Premium Editions at the official NIS America Store!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_247000"").data('bubble_data', {""name"":""PES: Peace Eco Smile"",""description"":""It takes place in Kichijoji, one of the most popular cities in Japan. Pes saves Kurumi when she almost falls into the pond of Iinokashira Park. Pes falls in love with Kurumi as soon as she kisses him. Pes starts working as a part-time in the flower shop and begins his adventures on Earth. \n \nCheck out the official site: http:\/\/www.toytoyota.com\/pes\/"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_201213"").data('bubble_data', {""name"":""Phantom Thief Reinya"",""description"":""\u201cPhantom Thief REINYA\u201d is the slapstick short comedy animation unfolded by a cute but spoiled thief and dumb police unit."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244288"").data('bubble_data', {""name"":""Phi Brain"",""description"":""Kaito, a puzzle-loving high school freshman, is selected as a candidate for \u2018PHI BRAIN.\u2019 He and his friend Nonoha find an \u2018unsolvable puzzle\u2019 near their school. The puzzle turns out to be a life-threatening \u2018philosopher's puzzle\u2019 created by the mysterious group P.O.G. (Puzzle Of God). After successfully solving the puzzle, Kaito is designated as a Solver and is joined by other Solvers as they battle P.O.G. all over the world by solving the \u2018philosopher's puzzles.\u2019"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243968"").data('bubble_data', {""name"":""Polar Bear Cafe"",""description"":""Polar Bear\u2019s caf\u00e9 revolves around a Canadian white bear that quits his boring job and starts a cafeteria near a zoo.\u00a0 He loves telling tall tales and always brags about himself.\u00a0 According to him, he was picked up by a human couple who owns a diner while he was drifting around on an iceberg.\u00a0  \r\n \r\nThough he has lost all contact with his Canadian family, he has discovered a new home in serving the diners\u2019 clientele, thanks to the kind couple.\u00a0 His caf\u00e9 is an embodiment of his personality.\u00a0 The place is always packed with many regulars, animals and humans, who are drawn by his charismatic magnetism."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240606"").data('bubble_data', {""name"":""Poyopoyo"",""description"":""Seemingly too cute for many, this will be a comedy for all ages and types! Follow this unique family and its spherical cat, in a warm-hearted comedy for the winter!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_109008"").data('bubble_data', {""name"":""Pretty Cure"",""description"":""Nagisa Misumi is an outstanding athlete and daring girl that doesn't take time to study but has a very strong sense of justice. She is one of the most popular students in her class.  \r\n \r\nHonoka Yukishiro is an honor-roll student always boasting the best grades in her class. At first glance, she may seem rather stuck-up, but actually she's just a bit spacey.  \r\n \r\nThe two are 8th graders at the Verone Junior High School for girls. Nagisa and Honoka each encounter Mepple and Mipple \u2013 two mysterious creatures who came down from the sky one night. Mepple and Mipple have fled from their homeland \u2013 the Field of Light \u2013 in order to escape an attack by the evil force of Dotsuku Zone. These two strange creatures grant Nagisa and Honoka the power to transform into superheroes called \""Cure Black\"" and Cure White\"", and thus the two girls become guardians of planet Earth. \r\n \r\nUsing their super powers, the two girls with contrasting lifestyles and personalities work together to battle the evil enemies sent to conquer Earth by Dotsuku Zone.  \r\n \r\nBut will they be able to save our planet? \r\n \r\n\u00a9TOEI ANIMATION. ALL RIGHTS RESERVED"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_242294"").data('bubble_data', {""name"":""Puella Magi Madoka Magica"",""description"":""She has a loving family and best friends, laughs and cries from time to time\u2026 Madoka Kaname, an eighth grader of Mitakihara middle school, is one of those who lives such a life. One day, she had a very magical encounter. She doesn't know if it happened by chance or by fate yet. This is a fateful encounter that can change her destiny- This is a beginning of the new story of the magical witch girls-"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_119010"").data('bubble_data', {""name"":""Queen's Blade"",""description"":""In a land where a queen is chosen every few years solely by winning a tournament, there can be no short supply of formidable opponents. For one woman warrior however, an early defeat clearly shows her that she is lacking in experience though she may be bountiful in body.  \n \nFortunately, while defeat could spell one's doom, her life is saved by a powerful stranger. But unfortunately for this savior, less-than-pure motives and shrewd family members mean her reward is a prison cell. Her release is prompt when the unseasoned warrior she saved, tired of her current lifestyle of nobility, sets off to prove herself."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244420"").data('bubble_data', {""name"":""Queen's Blade Rebellion"",""description"":""Set after the events of QUEEN'S BLADE, Gainos and the entire Continent has fallen under a tyrannical rule led by Claudette, the Thundercloud Queen and victor of the last Queen's Blade tournament. Annelotte, a brave young knight and exiled princess, leads a band of rebels called the Rebel Army to overthrow Claudette and restore peace and order to the Continent."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234111"").data('bubble_data', {""name"":""R-15"",""description"":""At a school full of geniuses from varying fields of study, Taketo Akutagawa has a special \u2013 albeit different \u2013 genius as well: writing harem novels. Now amongst the other geniuses, he aims to win the interclass competition and be recognized as the world\u2019s greatest harem writer.\u201d"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_88344"").data('bubble_data', {""name"":""Ramen Fighter Miki"",""description"":""In a world where terror grips the land and innocents have no solace from violence and brutality, one girl walks the line between life and death to bring her own special brand of... Ramen! Miki Onimaru works at her parent's Ramen shop, making deliveries, waiting tables, and attracting customers with cute, girlish charm.  \n \nSomehow, Miki can't complete even one of these simple tasks without the full employment of her extensive bone-crunching, skull-splitting martial arts moves and penchant for unleashing relentless destruction. Can Miki get through the day without messing up and angering her Mom, who makes her look like a gentle kitten in comparison?   \n \nCatalog Number: AWDVD-0715"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47700"").data('bubble_data', {""name"":""REBORN!"",""description"":""Tsuna, a timid junior high student, is a failure at school, sports, and social life. But everything around Tsuna has been completely changed when a baby called Reborn, who claims to be an Italian hit man from Vongola family shows up! Reborn was sent to groom Tsuna for his future life as a mafia boss of the family!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240620"").data('bubble_data', {""name"":""Recorder and Randsell"",""description"":""On the surface it seems as if Atsushi is an adult, but really he's just an elementary student!  However, he is paired with a second-year high school student Atsumi, whose height is just the opposite of his. These two together create an unusual relationship and will undoubtedly instigate some comedic antics in this slice of life comedy!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_224835"").data('bubble_data', {""name"":""Rio Rainbow Gate"",""description"":""The \""Howard Resort Hotel\"" is an entertainment destination where people gather from around the world to grab huge fortunes. In the casino is a beautiful female dealer named Rio Rollins, known far and wide as the \""Goddess of Victory\"". In order to approach closer to her mother, one of history's greatest dealers, she does battle to gather up the legendary cards called \""gates\"". Those who gather all 13 gate cards are presented with the title MVCD (Most Valuable Casino Dealer), proof that they are a top dealer. Set in a vast resort, an exciting battle begins with rival dealers that'll take your breath away! Throw in some \""supreme comedy\"" and a story that makes you cry when you least expect it, these cute and sexy girls will explode off your screen! With everyone's cheer of \""Leave it to Rio!\"", Lady Luck'll be with you, too!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234122"").data('bubble_data', {""name"":""Sacred Seven"",""description"":""Alma Tandoji lives a lonely life. One day, Ruri Aiba, a girl accompanied by her butler and maids, visits him. Knowing the power of Sacred Seven is latent within Alma, she asks him to lend her his powers. However, he refuses and drives her away since he injured many with his unusual strength in the past. \r\n \r\nMeanwhile, a fiendish Dark Stone creature suddenly appears in this peaceful town port in the Kanto region. Only Alma\u2019s power of Sacred Seven can fight against it. But Alma just lets his power run amuck and things begin to get worse. Ruri raised her gemstone in order to release his true abilities, \u201cMy Soul\u2026I give to you.\u201d \r\n \r\nWith Ruri\u2019s wishes engraved in it, will Alma be able to defeat the Dark Stone?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244002"").data('bubble_data', {""name"":""Saint Seiya Omega"",""description"":""Saints are champions of hope who have always appeared since the Age of Myth whenever evil threatens the world.  They would clad themselves in armor called Cloths, and fight to protect Athena, the goddess who rules the world's surface.  Seiya the Pegasus Saint has saved Athena many times, and while he and his friends are Bronze Saints, which is the lowest rank, their battles have been passed down as legend.   \r\n \r\nThe god of war and guardian of his namesake planet, Mars, was once sealed away by Seiya, but time has passed and his revival is at hand.  Meanwhile, Saori Kido (Athena) is raising the boy Koga, whose life Seiya saved, and he's been training every day to become a Saint in order to prepare for the coming crisis...  Unaware of his destiny, when Koga awakens to the power of his Cosmo hidden inside him, the curtain will rise upon the legend of a new Saint!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_224831"").data('bubble_data', {""name"":""Saint Seiya The Lost Canvas"",""description"":""The \""Holy War,\"" a battle fought between the Goddess Athena and the Underworld King Hades, has been taking place since the age of legends. This story takes place in the 18th century - far before the era of \""Saint Seiya.\"" It is a new story involving the Pegasus Saints, the Underworld King Hades, and the Goddess Athena! When Hades finishes his giant painting that covers the sky, \""The Lost Canvas,\"" it is said that all lives on Earth will be lost. Tenma, who became a Pegasus Saint, wishes to stop his best friend Alone, who was chosen to become a vessel for Hades. Look out for him as he heads for Hades' castle!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_130082"").data('bubble_data', {""name"":""Saki"",""description"":""Saki tells the story of a girl's mental and emotional growth as she and her fellow teammates battle fearsome rivals, all gunning for the national high school mahjong tournament, and is a cross between sports action and high school drama."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243976"").data('bubble_data', {""name"":""Saki Episode of Side A"",""description"":""Let's all have fun together again. \nAnd let's go to the nationals.  Come play with Nodoka! \n \nShizuno Takakamo and Ako Atarashi, members of the Achiga Girl's Academy Kid's Mahjong Club, invite the new transfer student from Tokyo, Nodoka Haramura, to come play with them. Kuro Matsumi, a student one year above them, and Harue Akado, a university student who is their coach, are there as well, and together they spend their days having fun and playing Mahjong.  But when it's time to go to middle school, Harue leaves for a pro team, and Ako ends up going to a different school.  And then Nodoka transfers, and the group is torn apart.  Time passes, and one day in her last year of middle school, Shizuno is watching TV when she sees Nodoka winning the national middle school Mah-jong tournament.  \n\""I want to play Mahjong with Nodoka again.\"" \n \nShizuno and Ako both decide to go to Achiga Girl's Academy High."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_192644"").data('bubble_data', {""name"":""Sasameki Koto"",""description"":""Without ever saying a word to her, honor student Sumika Murasame has had a crush on her best friend, Ushio Kazama since middle school. \n \nIn their social circle are the energetic members of the brass section: Kiyori Torioi, girls fashion magazine model Masaki Akemiya, and finally Tomoe Hachisuka and Miyako Taema, a couple, all making up a collection of eclectic personalities.  \n \nOne day, the idea of forming an all-girls after school comes up amongst them..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_170864"").data('bubble_data', {""name"":""School Days"",""description"":""Will Makoto win his love by taking a picture of Kotonoha without anyone knowing?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_185412"").data('bubble_data', {""name"":""Seitokai No Ichizon - Student Councils Discretion"",""description"":""The private high school Hekiyo Gakuen selects its student council members by a special system; by pure \u201cpopularity vote.\u201d  As a result, \u201ccute chicks\u201d are selected as Council members. But is it truly appropriate to choose those who will take the Council\u2019s important positions just by their popularity and looks? To resolve this problem, the \u201chonor student system\u201d is also adopted. A top student from each grade can participate in the Council.  \n \nKen Sugisaki, to be the only male member in the \u201ccommunity of pretty girls,\u201d and satisfy his secret ambitions, he manages to achieve the top mark at the year-end exam. Now, ahead of him waits the Council where beautiful girls with unique personalities gather. Can Sugisaki build his ideal Harem!?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229042"").data('bubble_data', {""name"":""Sekai Ichi Hatsukoi - World's Greatest First Love"",""description"":""Ritsu Onodera quit working for his father\u2019s company and transferred to work at the publishing company, Marukawa Shoten.  To his dismay, he's assigned to edit shojo manga in which he has no interest or experience. He works with a strict boss, Takano, that he has trouble getting along with from the beginning. But, Onodera later discovers that he knows Takano from his past..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243970"").data('bubble_data', {""name"":""Sengoku Collection (Parallel World Samurai)"",""description"":""Sengoku Collection revolves around many samurai who are accidentally removed from a parallel universe inhabited by well-known historic characters. Unlike the historical war period known to us, all inhabitants in this unique world look like high school girls. Coming from the medieval era and finding themselves totally amazed by everything they encounter in modern day Tokyo, the girls become best friends through their adventures."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_179111"").data('bubble_data', {""name"":""Sherlock Hound"",""description"":""This story takes place at the end of the 19th century in London. A famous dog detective, Sherlock Hound, was relaxing with his pipe... Dr. Watson is, of course, with Sherlock. The night is very foggy. From the light of a street gas lamp, one can only see for a few yards. From the distance, a coach comes dashing towards Baker Street and stops in front of Hound's flat. Someone violently knocks at the door. \""Watson, something's afoot!!\"" Hound in his well-known deer stalker hat and Inverness cape, leaves his flat in his prototype Benz with Dr. Watson. At the same time, Professor Moriarty, who calls himself the Napoleon of the criminal world, is in the middle of planning an evil plot with his followers, Todd and Smiley. \n \nWell, well, what is going to happen next? \n \nEnjoy signature directing styles from Hayao Miyazaki! \n \n \nFollow us on Twitter: https:\/\/twitter.com\/TMS_on_CR"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_192243"").data('bubble_data', {""name"":""Shin Koihime Musou"",""description"":""Towards the end of the Han Dynasty, the world is in the throes of chaos and disorder. \nIn a time when strength is needed, a hero is secretly sharpening her skills as a warrior of justice: her name is Kan'u Unch\u014d.  Kan'u will put her life on the line, test her strength and power, fighting to protect those in need."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_206451"").data('bubble_data', {""name"":""Shin Koihime Musou - Otome Tairan"",""description"":""During the last years of Han Dynasty, the world was filled with chaos and disorder.  Three girls, Ryubi, Kan\u2019u, and Chohi pledge to be blood-sisters in order to settle this world.  After overcoming many hardships, they were finally able to bring back peace.  However, a new evil creeps into their momentary peace.  The battles of Muso girls are about to start once again."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243318"").data('bubble_data', {""name"":""SHOTanime"",""description"":""SHOTanime reviews various manga and anime series in one short punch, and never says enough to spoil anything for the viewer. For more detailed discussion-like videos, SHOTanime also does series ending reactions, who are ONLY intended for viewers who have already completed the series. She also teaches a Japanese word or phrase, for those who are studying Japanese! Feel free to suggest a series for her to review!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_42860"").data('bubble_data', {""name"":""Shugo Chara"",""description"":""Amu Hinamori is a 5th grade transfer student who is fashionably cool, tough and independent. Despite this, she is also a girl who seems unapproachable, both at home and at school.  In reality, the \""Cool and Spicy\"" role she plays is just a facade that she unconsciously plays. With her guardian characters, she fights the Easter Company who is extracting people's eggs to create X Eggs & X Characters in search of a special egg called the Embryo."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_89637"").data('bubble_data', {""name"":""Sin The Movie"",""description"":""It is the near future, and the city of Freeport teeters on the verge of total collapse. The twin tides of rampant crime and ruinous graft face only one barrier: the elite strike force HARDCORPS. Led by Colonel John Blade, HARDCORPS is the last force for justice in the darkening city.  \n \nIn SIN, Blade must unravel a series of mysterious kidnappings. As he delves into the city\u2019s merciless underworld, an elaborate mystery unfolds; at its heart, the SinTEK corporation and its leader, the ruthless and beautiful Elexis Sinclaire. A brilliant biochemist, Sinclaire will stop at nothing to achieve her goal: a plan that could force the next step of human evolution\u2014or spell doom for mankind!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229060"").data('bubble_data', {""name"":""SKET Dance"",""description"":""At Kaimei High School, the Living Assistance Club (aka the Sket Brigade) was organized to help students with problems big or small. Most of the time, though, they hang out in their club room, bored, with only a few trivial problems floating in every once in a while. In spite of this, they still throw all their energy into solving these worries."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_182853"").data('bubble_data', {""name"":""Sketchbook"",""description"":""From the creators of \""Rozen Maiden\"", \""Basilisk\"",\""HAMTARO\"" and \""Kyo Kara Maoh!\"" A popular comic strip is now a TV animation series! \n \nA high school girl, Sora Kajiwara, joins the art club. Sora carries around a sketchbook to wherever she goes, in order to capture cherished moments. She asks herself, \""If I really wanted to remember the moments in everyday life, and to seize every moment of all the events that happen, why not use a camera or video camera?\"" However, it just isn't the same. What's not shot, what's not spoken, what's not heard, but what is left in her drawings; which flourishes and blooms inside her heart. \n \nThose who are a bit tired of the busy everyday life will enjoy rediscovering the beauty of ordinary things such as animals, plants, and the changing colors and scenery of the sky."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_105505"").data('bubble_data', {""name"":""Skip Beat!"",""description"":""The story of Skip Beat! is about Kyoko Mogami, a wistful yet cheery sixteen year-old girl who loves her childhood friend, Shotaro, but is cruelly betrayed and thus seeks revenge against him."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_47822"").data('bubble_data', {""name"":""Slam Dunk"",""description"":""Hanamichi Sakuragi, an entering freshman at Shohoku High, holds a record for being rejected by 50 girls during middle school. Ever since the last girl turned him down for a guy on the basketball team, Sakuragi's been traumatized by the sport."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246612"").data('bubble_data', {""name"":""So, I Can't Play H!"",""description"":""High schooler Ryosuke Kaga is also known as Erosuke for his extreme adolescent interest in the erotic side of life. One day he encounters a young girl standing alone in the rain. She is Lisara Restor, an elite Death God who has come to Earth in search of a human known as the \""Singular Man.\"" Ryosuke ends up making a contract with Lisara, allowing her to suck the energy she needs for her activities on Earth from him. And it turns out the source of that energy is his own perverted spirit!  \r\n \r\nAs a result Ryosuke can no longer feel aroused in the heat of the moment. After trying every erotic method he can think of, he finds he has no choice but to help Lisara in her search in order to recover his perverted spirit.\u201d"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_199622"").data('bubble_data', {""name"":""Soranowoto"",""description"":""There is no happiness in the world. \nThere is no joy. If anything, it is a dark and needy world. \nHowever, that is just one state of the world... \nThere are beautiful things, filthy things, harsh things, and even enjoyable things...How you take them will be up to you?"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243964"").data('bubble_data', {""name"":""Space Brothers"",""description"":""Once upon a time there were two brothers, Hibito and Mutta. Hibito, the littler brother, had been blessed with good luck since the day he was born. Mutta, the elder, had always had bad luck.  One day, the two brothers saw a UFO. This experience made them decide to become astronauts.  And while Hibito succeeded, Mutta went to work for a car company.  Then he caused an incident and had to quit. And then just as Hibito is about to fly to the moon... \r\n \r\nTo follow his brother Hibito to the moon, Mutta will attempt to become an astronaut at the age of 32.  Unaware of his own talent, elder brother Mutta chases his dreams to get back in front of his younger brother.  Believing the worlds: \u2018The older brother should always be ahead of the younger one...\u2019"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_62092"").data('bubble_data', {""name"":""Speed Racer"",""description"":""Speed Racer is a teenager who drives a special sports car, the Mach 5, in races throughout the world. The Mach 5, designed by his father, a retired professional racer, contains features that enable Speed to drive over any terrain and through all obstacles. Speed has to use them often, since he frequently meets unscrupulous competitors and international criminals who try to fix the races in their own favor. Speed is often accompanied by his girl friend Trixie, his kid brother Spridle, and Spridle's pet chimp Chim-chim. The fast-paced adventures of this brave foursome, in exotic settings around the globe, prove that quick wits and fair play are always sure winners! \r\n-AniDB"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_219881"").data('bubble_data', {""name"":""Squid Girl"",""description"":""With the environment ever deteriorating on a global scale, rage of those who suffer the most is about to erupt.  \nTake this girl, for example. She is hell-bent on annihilating the mankind before her species is wiped off the surface of the earth, or, to be precise, off the bottom of the ocean.  \n \nAs she sets out on her vengeful journey, however, she becomes aware of certain limitations. She\u2019s nothing but a squid out of water\u2026"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_236076"").data('bubble_data', {""name"":""Star Driver"",""description"":""Star Driver takes place on the fictional Southern Cross Isle. One night, a boy named Takuto washes up on shore swimming from the mainland. He later enrolls in Southern Cross High School as a freshmen and makes new friends. However, beneath the school is a group of mysterious giants called Cybodies, which can be controlled by humans in an alternate dimension known as Zero Time. Takuto, The \""Galactic Pretty Boy\"" , finds himself dragged into opposition with the \""Glittering Crux Brigade\"". Glittering Star Cruciform Group), a mysterious group that intends to take possession of the island's Cybodies for their own purposes as well as break the seals of the island's four Shrine Maidens, whose powers prevent the Cybodies from functioning outside of Zero Time."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_130084"").data('bubble_data', {""name"":""Steel Angel Kurumi"",""description"":""A boisterous comedy starring the perfectly invincible \""Maidroid\"" Kurumi, a product of science and magic and boy named Nakahito, they manage to get into all sorts of trouble..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_144882"").data('bubble_data', {""name"":""Steel Angel Kurumi 2 Shiki"",""description"":""Once upon a time, in the \""Taisho\"" era (1912-1928), there were some totally invincible, beautiful girl-shaped androids, called the Steel Angels, who obeyed their master's orders absolutely.Time has passed since then and in present day Japan...  \n \nA bespectacled junior high school girl Nako, the beautiful shrine maiden, comes across \""Canwan\"", the steel android dog, in the basement of her home. By accident she also unseals the android, \""Steel Angel KURUMI 2 Shiki\"".  \n \nNako's pal from childhood, Uruka is a daughter of Tenkai Sumeragi, leader of the great Sumeragi group which even controls the Prime Minister of Japan.Uruka is secretly in love with Nako and she has been waiting for her chance to steal Nako's first kiss, but Kurumi the android, beats her to it, much to the dismay of Uruka. \n \nUraka's father promises to take revenge on KURUMI and he employs all means possible to do so.During these battles, Uruka accidentally uncovers another Steel Angel called\""SAKI 2 Shiki\"" , then yet another Steel Angel called \""KARINKA 2 Shiki\""which somehow follows after KURUMI and SAKI.  \nA series of severe battles over Nako take place, marking the end of any semblance of a peaceful life for her. Nako wonders \""How come I am their master?\"" \""What are the Steel Angels, anyway?\"" \""Who made them, for what purpose?\"" \""Is KURUMI really an angel to me? What else could she be?\"""",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_53620"").data('bubble_data', {""name"":""Steel Angel Kurumi Zero"",""description"":""Sometime in the distant future, Kurumi, Saki and Karinka share an apartment with another girl named Excelia. One day, after school, Kurumi tells everyone that she is in love with a man she met named Michihito Kagura, but she doesn't know when she will see him again."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229050"").data('bubble_data', {""name"":""STEINS;GATE"",""description"":""Steins; Gate follows an eclectic group of individuals who have the ability to send text messages to the past. However throughout their experimentation process, an organization named SERN who has been doing their own research on time travel tracks them down.  Now it\u2019s a careful game of cat and mouse to not get caught and moreover, try to survive."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_110666"").data('bubble_data', {""name"":""Sumiko"",""description"":""Sumiko is a latch-key kid living in a suburb of Tokyo, where she is somewhat of a local celebrity.  She is 8 year's old and dreams of becoming a famous singer some day.  Her Mom works at a neighborhood ramen shop and Sumiko hangs out there a lot after school.  Her Papa is a truck driver who is often away from home.  However, when he is at home, she loves to accompany him to the local Karaoke box, where she can practice singing her favorite tunes.   \n \nThe Sumiko music videos are a preview of the Sumiko anime series.  If all goes well, we will get to enjoy following Sumiko as she ventures through life.  \n \nThe creators of Sumiko are Uruma-Delvi.  Uruma-Delvi have been creating Anime for a long time and they are most famous for their smash hit, Oshiri Kajiri Mushi or Bottom Biting Bugs."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_219963"").data('bubble_data', {""name"":""Super Robot Wars OG The Inspector"",""description"":""The era known as \""New A.D.\"" \r\nSix months since the DC War, where the Divine Crusaders rose up against the Earth Federation Government, and the \""L5 Campaign\"", against the alien Aerogaters..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246948"").data('bubble_data', {""name"":""Sword Art Online"",""description"":""In the near future, a Virtual Reality Massive Multiplayer Online Role-Playing Game (VRMMORPG) called Sword Art Online has been released where players control their avatars with their bodies using a piece of technology called: Nerve Gear. One day, players discover they cannot log out, as the game creator is holding them captive unless they reach the 100th floor of the game's tower and defeat the final boss. However, if they die in the game, they die in real life.  Their struggle for survival starts now..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_221268"").data('bubble_data', {""name"":""Tantei Opera Milky Holmes"",""description"":""\""Sheryl, Nero, Elly, and Cordelia are the most popular students at Holmes Detective Academy, because of the many cases they've solved with their Toys and teamwork.  The students and the people of the Detective City Yokohama all look up to the members of \""Milky Holmes\"", as they're called.  But on a dark and stormy night, during a battle with their rivals \""The Gentlemen Thief Empire\"", they lose their toys, and their fate undergoes a dramatic change...\"""",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246840"").data('bubble_data', {""name"":""Tari Tari"",""description"":""Too young to be adults, but no longer children... \r\n \r\nWakana Sakai was involved in music, but gave it up one day. Konatsu Miyamoto loves singing and can't be torn from it. Sawa Okita would do anything for her closest friends. They laugh, they fight, they worry, they love... Through their very ordinary lives, little by little the girls learn to move forward. Sometimes they feel as if they can't go on alone, but as long as they have their friends, they believe they'll make it someday. Wakana, Konatsu, Sawa, and the music they make in their ensemble weave a tiny but dazzling story of the power of music. \r\n \r\nThe last summer of high school... It's too soon to give up on dreams. The song echoing throughout Enoshima continues to give us courage today."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_191638"").data('bubble_data', {""name"":""Tegami Bachi Letter Bee"",""description"":""Gauche Suede is on his last delivery before a big promotion. In the outskirts of Yodaka, the darkest area of Amberground, Gauche is surprised to find that the package is a young boy named Lag Seeing. Lag had been traumatized by his mother's abduction and is due to be delivered to his aunt. In this remote area rife with Gaichuu, Lag and Gauche face a dangerous journey that inspires Lag to become a Letter Bee."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246428"").data('bubble_data', {""name"":""The Ambition of Oda Nobuna"",""description"":""Ordinary high schooler Yoshiharu is sent back in time to the Warring States Period, however not the same time line that he remembers. There he meets Nobuna Oda - not Nobunaga, Nobuna. In this world, all the famous figureheads and warlords of the era are female! Nobuna teams up with Yoshiharu to help fuel her ambition and quest to rule the world\u2026"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_48808"").data('bubble_data', {""name"":""The Diary of Tortov Roddle"",""description"":""From Academy Award-winning animator Kunio Kato comes THE DIARY OF TORTOV RODDLE, a tapestry of whimsical fantasy featuring tatters of everyday life of the eponymous Tortov Roddle.  Riding on the back of his pig with legs as long as stilts, what will be the next surreal journey to be written in his diary? \n \nFollow Tortov as he journeys through very surreal, magical, picturesque landscapes, meeting interesting characters and circumstances on the way. Accompanied by his long-legged pig friend, Tortov takes us on an on-going adventure of peaceful contemplation. \n \n(from ann) \n \nEpisode 1 to 6 is the flash animation and originally made for website and released in 2003. \nEpisode 7 \""Red Berry\"" is the fully animated version of the stories and released in 2004."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234109"").data('bubble_data', {""name"":""The Idol Master"",""description"":""THE IDOLM@STER follows 13 girls from the 765 Production Studio, whose sole goals is to become the top idols in the Japanese entertainment industry. Along with the laughs, struggles and tears that are inherently part of this journey, you will cheer for the girls of IDOLM@STER as they climb their way to the top!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240890"").data('bubble_data', {""name"":""The Knight in the Area"",""description"":""The Knight in the Area tells the stories of middle and high school students as they grow up through their soccer clubs, compete in national tournaments, and strive for world titles. Main character Kakeru, his respected elder brother Suguru, and childhood friend Nana Mishima, along with all the teammates, rivals from other schools, and world class soccer players around him radiate their own brilliant light as they face adolescence with all their strength. The story also incorporates the strong bond between Kakeru and Suguru, the love of their family, the friendship of their teammates and rivals, and fleeting first loves... all coming together to ignite passion in the hearts of all viewers. \r\n \r\nThe soccer depicted in The Knight in the Area does not rely upon the efforts of \""super athletes\"" or nonsensical \""special techniques,\"" but illustrates \""fanciful play\"" as an extension of reality. Viewers will be drawn to this reality-based anime and its powerful scenes unfolding in realistic settings. Explanations of soccer rules and fundamentals are carefully presented as the story develops. It attempts to raise interest and understanding of elements such as formation diagrams for those unfamiliar with soccer, while depicting soccer in a way that enthusiasts can appreciate as the story of Kakeru's growth unfolds. The Knight in the Area appeals to a broader range of viewers than any anime ever has before. \r\n \r\nKakeru is supported by his childhood friend Nana. Though she is the team manager, Nana is a soccer prodigy known as the Little Witch who goes on to join the Nadeshiko Japan women's soccer team. The Knight in the Area had its eye on the team even when it was still being serialized in Magazine, before women's soccer gained its current popularity. The anime's depiction of Nana's activity in Nadeshiko Japan is consistent with the present time."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_245832"").data('bubble_data', {""name"":""The Legend of Qin"",""description"":""The Legend of Qin tells the story of the rise and fall of the Qin Dynasty, the first empire of China. During this time, the six kingdoms have been conquered and brought under the auspices of the Qin Empire, which is ruled by the iron fist of Emperor Ying Zheng. Despite this ruthless takeover, pockets of resistance stand opposed to Qin rule and fight for freedom. It is against this backdrop that we meet Tianming, a young boy who is being hunted down by the Qin Empire for reasons beyond his knowledge. Together with his friends, allies, and teachers, Tianming will change the course of history."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_239176"").data('bubble_data', {""name"":""The Live Show"",""description"":""The Live Show is a live streaming broadcast covering news in the anime and pop culture field, as well as releasing exclusive interviews with some of the hottest people in the industry!!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_46566"").data('bubble_data', {""name"":""The Melancholy of Haruhi Suzumiya"",""description"":""Not long after the entrance of the school, Suzumiya Haruhi introduced herself in a strange way. The classmates wondered whether she was serious or just kidding."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_124056"").data('bubble_data', {""name"":""The Melody of Oblivion"",""description"":""This takes place in the 20th century where the Monsters have succeeded in defeating the humans through a violent war. The Monsters rule the earth in the 20th century with no one recalling what had happened in the past. Bocca, is a teenage boy who chooses the path of becoming the Warrior."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234114"").data('bubble_data', {""name"":""The Mystic Archives of Dantalian"",""description"":""Hugh inherits a musty old mansion, along with the entire book collection contained within, from his grandfather - a bibliomaniac who once traded half his lands in exchange for a copy of a rare book. The only condition was that he also inherit the \""Archives of Dantalian.\"" \r\n \r\nHaving arrived at the mansion to put his grandfather's possessions in order, Hugh meets a girl of 12 or 13 quietly reading amongst the tall piles of books in the basement.  \r\nShe is dressed in jet-black, and wears a lock around her neck. Her name is Dalian, and she controls the gateway to the Archives of Dantalian, where the forbidden mystic books of demonic wisdom are kept..."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_240608"").data('bubble_data', {""name"":""The Prince of Tennis II"",""description"":""Middle school students fought fiercely against one another in a national tournament. \nNow, 50 chosen representatives return to fight again as high school students! \n \nThe stage is the U-17 (Under Seventeen) Japanese Representative Training Camp. \nMiddle school students have been invited for the first time to a camp known to produce the best Japanese tennis players. National champions from Seigaku, Hyotei, Rikkai, Shitenhoji... With all the rivals from the national tournament attending, everyone eagerly awaits their reunion. \n \nAt first, the middle schoolers do not expect much from their high school opponents. However, the training camp is not so easy! \n \nAthletes with skills incomparable to those they previously faced and their mysterious coach appear before them. Faced with numerous demanding challenges, can the middle schoolers survive in this meritocratic training camp? \n \nThe new chapter of \""Prince of Tennis\"" begins here! \nThe curtain rises on a new stage--"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_62208"").data('bubble_data', {""name"":""The Tower of DRUAGA -the Aegis of URUK-"",""description"":""The long awaited \u201cSummer of Anu\u201d has come. \r\n \r\nIt is a season that comes but once every few years and during which the powers of the monsters within the Tower wane thanks to the grace of the great god Anu. \r\n \r\nEach Summer of Anu, the armies of the Uruk Kingdom secure their strongholds within the Tower, aiming to eventually conquer the upper floors. \r\n \r\nDecades of re-fortification and battle have passed since the infamous adventures of Gil and Ki. Their story has now become a legend. \r\n \r\nNow, finally, the third Summer of Anu has arrived. \r\n \r\nThe city of Meskia - the first stronghold built on the first level of the Tower - is brimming with unprecedented expectation. In addition to the Uruk Army, preparing for their third campaign against Druaga, innumerable adventurers have been drawn to Meskia by rumors of a legendary treasure believed to be hidden on the top floor of the Tower... \r\n \r\nTranslation and subtitles by GDH and BOST \r\n(C)NBGI\/Izumi Project"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_119016"").data('bubble_data', {""name"":""The Tower of DRUAGA -the Sword of URUK-"",""description"":""Why do people climb the Tower for the things they seek?  \r\n \r\nIt has been 80 years since the legend of King Gilgamesh.  \r\n \r\nThe impregnable walls of the Tower of DRUAGA have returned again, along with a new evil force.  \r\n \r\nJil and his legendary band of followers must journey to the top of the Tower of DRUAGA, defeat the evil forces that lie ahead of them and obtain the mysterious treasure known as the \""Blue Crystal Rod.\""  \r\nHowever, Jil might lose everything in the process.  And what is his beloved Kaya seeking? And what could she possibly wish for once they obtain the \""Blue Crystal Rod?\"" \r\n \r\nWith broken spirits and enigmatic questions that hold no answers lingering, Jil is still trying to figure everything out.  Then, a mysterious girl named Kai appears before him and says: \u201cTake me to the top of the tower.\u201d  \r\n \r\nKai\u2019s request shrouded in ambiguity, Jil will have another chance to work towards completing his destiny and ascend the Tower.  With his hopes and aspirations seemingly slipping out of his hands, Jil must rise to the challenge once again on this never-ending adventure."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_219879"").data('bubble_data', {""name"":""The World God Only Knows"",""description"":""Keima, a high school student, is an avid player of romantic simulation games.  He is known on the Internet as the \""Divine Capturer\"" for his legendary skills to \""capture\"" any 2D girl in games. In his real school life, Keima is considered nothing but a gloomy geek with thick glasses. \n \nKeima receives an e-mail offering him a contract to \""capture\"" girls.  When he accepts it thinking it is an invitation to a game play, a demon from hell nicknamed Elsee shows up.  She asks for his cooperation to help her in hunting evil spirits on the run.  These spirits hide themselves inside the lonely girls' heart, and Elsee suggests that the only way to force the spirits out is to \""capture\"" their hearts, by making them fall in love and filling up the hollows which the runaway spirits hide in.  Just the kind of job for the Divine Capturer! \n \nInterested only in 2D girls, however, Keima is appalled by the idea and refuses the assignment.  He has no romantic real life experiences whatsoever.  Nevertheless, with the contract already executed, Keima has no choice but to help Elsee no matter what; if they fail, it is Elsee and Keima who will lose their heads."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_248190"").data('bubble_data', {""name"":""TheInsaneGamefreak"",""description"":""Hi I'm an anime reviewer on youtube known by many names. TheInsanegamefreak, Thegamemanic, Terrellgamevlogs Or....you can just call me Terrell. I'm 21 and I'm a watcher, collector, and reviewer of anime. I delve into all genres and I give you a honest opinion on these shows, good or bad. I hope you enjoy the content. Life's a Game, Play to Win, Peace-O!! \r\n \r\nSubscribe:  \r\nhttp:\/\/www.youtube.com\/user\/TheInsaneGamefreak  \r\nTwitter: http:\/\/twitter.com\/TheGameManic Email: Thegamemanic@yahoo.com  \r\nVlog Channel: http:\/\/www.youtube.com\/user\/TerrellGameVlogs  \r\nSkype: the123gamefreak  \r\nMAL: http:\/\/myanimelist.net\/profile\/TheGameFreak"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_87731"").data('bubble_data', {""name"":""Time of Eve"",""description"":""The \""Time of Eve\"" (Eve no jikan) series is the latest work by Yasuhiro YOSHIURA, who stunned the anime world with \""Pale Cocoon\"" and \""Aquatic Language\"" (Mizu no kotoba).  The series consists of six 15-minute episodes, released every two months starting August 2008. Produced by Studio Rikka and DIRECTIONS, Inc. \n \nTime of Eve Episode 6 went live on September 19, 2009, bringing the first season to a close. A huge \""thank you\"" to all you fans who endured the long waits between episodes, and kept the series alive with your enthusiasm and support. Time is Eve is for all of you out there! Be sure to stay tuned for news on the series! \n \nAs of December 2011, the movie version of \""Time of EVE\"" is available for purchase & rental from the iTunes Store (US, Canada, Australia, and New Zealand)."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_213234"").data('bubble_data', {""name"":""Tono to Issho"",""description"":""A new bunch of gags (\""gyags\"" in Japanese) every week...and one of the guest stars is none other than the world famous Gackt!!!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246430"").data('bubble_data', {""name"":""Total Eclipse"",""description"":""In 1973, the invasion of an extra terrestrial life form, the BETA, began a war that has driven mankind to the brink of extinction. In an attempt to counter the BETA\u2019s overwhelming strength in numbers, mankind has developed the humanoid weapons known as TSFs, deploying them on the front lines of their Anti-BETA War all across the globe. However, mankind still lost the majority of Eurasia to the superior numbers of the marching BETA. For nearly 30 years mankind has remained bogged down in its struggle against the BETA with no hope in sight. \n \nIn 2001, development of next-generation TSFs has become a problem for Imperial Japan, who sustains the front lines on the Far-East. As a solution, the UN proposed a US-Japan joint TSF refurbishment plan as part of their \u201cProminence Project\u201d. Yui Takamura, a member of the Imperial Royal Guard, is placed in charge of this project and heads to Alaska. Meanwhile, Yuuya Bridges, a young soldier in the US Army, makes his way for Alaska as well. \n \nLittle did either of them know that their encounter would forever change their destinies\u2026 \nWitness this exciting tale of human drama and robot action as mankind faces the brink of extinction"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_248024"").data('bubble_data', {""name"":""Traveling Daru"",""description"":""Daru, a cherished handmade stuffed toy, gets lost and separated from its owner one day when at the airport. Determined to find her again, Daru sets out on a journey around the world in search of her. As time passes, the girl begins to forget about Daru. Will Daru be able to find and see his beloved owner again? \r\n \r\nCheck out CoMix Wave Films at http:\/\/www.facebook.com\/cwfilms! \r\n \r\nYou can buy the DVD on Amazon JP at http:\/\/www.amazon.co.jp\/gp\/switch-language\/product\/B0083LWKW4\/ref=dp_change_lang?ie=UTF8&language=en_JP"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244256"").data('bubble_data', {""name"":""Tsuritama"",""description"":""\u201cThe setting is Shonan, Enoshima. A town where the nostalgic and the fresh coexist. All his life, high school student Yuki has never had a friend of his own, because he's freakishly inept at communicating with others. Haru, a self-proclaimed alien from outer space, is trying to get him to go fishing. Natsuki is a local boy born and raised in Enoshima, who always seems to be annoyed by whatever's going on around him. Akira is a mysterious Indian boy who watches over them all, while maintaining a cautious distance. These four angst-filled teenagers meet and go fishing; and their tiny island takes center stage in an epic story...  \r\n \r\nThis is where it all starts - their SF (= Seishun Fishing) story!\u201d"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234116"").data('bubble_data', {""name"":""Twin Angel: Twinkle Paradise"",""description"":""Haruka Minazuki and Aoi Kannazuki are freshman high school students and best friends who enjoy school life like any other kid. But what's different about them is they live a different life at night fighting off demons and enemies."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_238468"").data('bubble_data', {""name"":""Un-Go"",""description"":""\""UN-GO\"" is a full-fledged detective story about a detective and handsome boy combo who tackle the world's most difficult crimes together for reasons of their own."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243972"").data('bubble_data', {""name"":""Upotte!!"",""description"":""A teacher who has just transferred into Seishou Academy soon finds himself in the hospital. This is not surprising, considering his students are assault rifles! Kitsune Tennouji of EDEN'S BOwY fame returns after 16 years with a surprising military comedy featuring attractive girls who are actually anthropomorphized guns! The girls are given special characteristics and personalities to match their gun types, and they spend their school days with a firearm in one hand, running rampant and occasionally shooting a boost of energy into their school life!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_206370"").data('bubble_data', {""name"":""Uraboku"",""description"":""Yuki Giou is a high school student with no relatives, living in an orphanage who ends up meeting a beautiful young man, Zess who somewhat inspires his nostalgia. We are on a journey to discover the past between these two men."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234151"").data('bubble_data', {""name"":""Usagi Drop"",""description"":""By force of circumstances, a 30-year-old single man with a full-time job suddenly starts raising a 6-year-old girl. While running each other ragged, the two of them gradually grow into a \u201cfamily\u201d. This is the long-awaited anime version of \u201cBunny Drop\u201d, the extremely popular and finely crafted comic by Yumi Unita. It\u2019s a heartwarming and entertaining work that naturally depicts child-raising experiences, but also the instantly relatable warmth and noise of being in a family."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_246966"").data('bubble_data', {""name"":""Utakoi"",""description"":""\u201cUtakoi is a historical romance story based on Ogura Hyakunin Isshu's interpretation of a collection of 100 romance poems that are used in traditional Japanese karuta card\/memory game \r\n \r\nUtakoi takes life based of the very liberal interpretation of the Hyakunin Isshu anthology \r\nof poems featuring 100 romantic poems from 100 different poets such as The Tale of Genji's Murasaki Shikibu. However this romantic \u2013 both in story and artistic design \u2013 series starts its focus with some twists and turns centered around the love between Fujiwara and her romantic interest Ariwara\u2026\u201d"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_212079"").data('bubble_data', {""name"":""Valerian and Laureline"",""description"":""The story is based on a well know Sci-Fi graphic novel written by renown French graphic novel artists, Jean-Claude Mezieres and Christin."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_222048"").data('bubble_data', {""name"":""Venus to Mamoru!"",""description"":""Responding to will of people, mysterious power called Beatrice makes everything possible. Beatrice is referred to as Portrayal of Magic and there\u2019s one and only school in Japan called Tokyo Beatrice University High School which teaches about it (This school is commonly known as TBU-High). Mamoru Yoshimura is a high school student who had his life saved by miracle of Beatrice in early childhood."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_59086"").data('bubble_data', {""name"":""Viewtiful Joe"",""description"":""With special superpowers bestowed by Captain Blue, Joe transforms into \""Viewtiful Joe\"" to bash the enemies in the movie world."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_238016"").data('bubble_data', {""name"":""Wagnaria!!"",""description"":""The family restaurant WAGNARIA stands by itself in Hokkaido.  Get ready to dish up some wacky comedy together with Sota and his coworkers!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_229051"").data('bubble_data', {""name"":""We, Without Wings - under the innocent sky"",""description"":""In the big city of Yanagihara, the masses of people and buildings make it a bustling place to exist, and yet, people will meet and fall in love here in this city."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_96462"").data('bubble_data', {""name"":""Web Ghosts PiPoPa"",""description"":""Net Ghosts PiPoPa (Web Ghosts PiPoPa) \r\n \r\nWeb Ghosts PiPoPa is a comedic, action adventure of a boy who is swallowed into his cell phone and transported to the virtual world of the internet, where he befriends three internet ghosts: Pit, Pot and Pat."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_191567"").data('bubble_data', {""name"":""White Album"",""description"":""Toya Fujii, the male protagonist, is a twenty year old college student dating a rising singer named Yuki Morikawa. Along the way, they are faced with numerous challenges throughout the course of events at Fuji\u2019s college. Be prepared for an exciting journey!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_170741"").data('bubble_data', {""name"":""Wonder Beat Scramble"",""description"":""In the year 2119, the Greensleeves, a spaceship investigating the existence of extraterrestrial life, encounters the rogue planet \""X-23.\""  After they discover traces of destroyed high-level civilizations on planets that X-23 had passed by, the World Federation orders the Greensleeves to destroy X-23.  However the captain of the ship, Professor Sugita Isao, refuses the order and all communications are cut off.  Two years later, the eldest son of Professor Sugita who lives in the science academy town Nagisa City, is suddenly lead to Phoenix Tower, an integrated health and science research institute.  The leader of the special medical team \""White Pegasus,\"" Dr. Miya, requests he joins them.  One after another of the citizens of Nagisa City fall ill for unknown reasons, and then are treated by a special vessel called the \""Wonder Beat\"" which is miniaturized and inserted directly into the affected area. And that is how the battle begins between the White Pegasus Team and the mysterious aliens who keep attacking them. \r\n \r\n(C)MushiProduction CO.,LTD."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_245922"").data('bubble_data', {""name"":""Xamd: Lost Memories"",""description"":""Combining hard-hitting mecha action and fantastic supernatural elements, XAM'D is a thrilling new benchmark in anime. Set on a peaceful island during a violent terrorist attack, a young boy is suddenly transformed into a metal-cased mercenary. But with this great power comes even greater danger. Aiyuki must discover how to master this remarkable new power-or risk having this mysterious fusion of rock, metal and magic destroy him!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_87576"").data('bubble_data', {""name"":""Yatler  Matler Space Tyrants"",""description"":""2 Aliens attempt to take over the Earth and the universe."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_119458"").data('bubble_data', {""name"":""Yokuwakaru Gendai Maho"",""description"":""Koyomi Morishita is a clumsy high school freshman girl looking to change herself. One day, she unexpectedly finds a brochure for a school of magic with an extremely powerful head magician, Misa Anehara. Misa specializes in magic related to computers and programming. Watch the magical fantasy unfold!"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_48680"").data('bubble_data', {""name"":""Yonna in the Solitary Fortress"",""description"":""Yonna, a girl with incredible powers, has been living in a deserted \u201csolitary fortress\u201d alone with her brother Stan, her heart shut from the world. Stan had desperately protected Yonna from those who come seeking her mysterious power until one day, a boy approaches the solitary fortress. Stan tries to kill this boy like all others before him, but the boy tries to take Yonna out of the fortress regardless of his own safety. \r\n \r\nThough the world of 3D computer graphics tends to lean toward sharp and cool visuals, Kengo Takeuchi has been searching for a softer, warmer atmosphere in 3D-CG. Takeuchi brings us this heart-warming 34-minute 3D-CG work after participating in Final Fantasy: The Movie with Square USA. With a commitment to creating CG with a unique atmosphere, Takeuchi has created the world of the girl named Yonna."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_237596"").data('bubble_data', {""name"":""You and Me"",""description"":""\""Friends since kindergarten and seemingly like blood brothers, You and Me follows the lives of Yuta and Yuki Asaba, Shun Matsuokan and Kaname Tsukahara; as well as transfer student Chizuru Tachibana who joins the circle of friends. Together we will watch as they laugh, dance, cry and share the memories of growing up together in everyday life.\u201d"",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_243966"").data('bubble_data', {""name"":""You and Me 2"",""description"":""\""No matter how many years go by, I'm sure we'll still be laughing together.\"" \r\n \r\n\""Twins Yuta and Yuki, Kaname, and Shun have been childhood friends since kindergarten. When transfer student Chizuru joins them, their five man school life becomes all the more lively. Through the changing seasons, the boys will find laughter, surprises, love, and new encounters waiting for them.\"" \r\n \r\n\""The second season of the boys growing a little every day of their invaluable daily lives is about to begin!\"""",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_244052"").data('bubble_data', {""name"":""Yurumates3Dei"",""description"":""Yurumates3Dei follows Yurume, an 18-year-old high school graduate who has yet to be accepted into college. In an effort to study for exams, she moves to Maison du Wish, an apartment complex on the outskirts of Tokyo which is full of other out-of-school \""r\u014dnin.\"" Maybe that has something to do with the rumor about Maison du Wish: no one who lives there passes the entrance exam."",""offsetLeft"":315,""offsetTop"":34});
          $(""#media_group_234145"").data('bubble_data', {""name"":""YuruYuri"",""description"":""YURUYURI follows four students who decide to occupy the room of defunct tea ceremony club, dubbing it the \u2018Amusment Club.\u2019 While the Student Council does its best to eliminate this club, their endless energy, happiness and comedy will keep audiences smiling all season long!"",""offsetLeft"":315,""offsetTop"":34});
      </script>
  <script type=""text/javascript"">
  videos.set('initial_document_scrolltop', $(document).scrollTop());
  videos.set('l.generic.titlecase.add_to_queue', ""Add to Queue"");
  videos.set('l.generic.titlecase.remove', ""Remove"");
  videos.set('user_queues_group_ids', []);
  videos.set('media_type_name', ""anime"");
  videos.set('order', ""alpha"");
  videos.set('more_url', ""\/videos\/anime\/alpha\/ajax_page"");

  $(videos.attach);
</script>

        </div>
                                <div id=""footer"" class=""cf container-shadow-bottom"">
  <div id=""footer_menu"" class=""clearfix"">
    <table style=""width: 100%;"">
      <tr>
        <td class=""footer-column"">
          <h6>Popular Shows</h6>
          <ul>
                        <li><a href=""/naruto-shippuden"" token=""bottombar"">Naruto Shippuden</a></li>
            <li><a href=""/bleach"" token=""bottombar"">Bleach</a></li>
            <li><a href=""/shugo-chara"" token=""bottombar"">Shugo Chara</a></li>
            <li><a href=""/blue-exorcist"" token=""bottombar"">Blue Exorcist</a></li>
            <li style=""padding-top:10px;font-weight:bold;"">
              <a href=""/comipo"" token=""bottombar"">Purchase ComiPo!</a>
            </li>
            <li style=""padding-top:10px;"">
              <a href=""/feed"" token=""bottombar"">
                <img class=""icon"" src=""http://static.ak.crunchyroll.com/i/rss.png"" alt=""rss"" />
              </a>
              <a href=""/feed"" token=""bottombar"">RSS</a>
            </li>
          </ul>
        </td>

        <td class=""footer-column"">
          <h6>Platforms and Devices</h6>
          <ul>
            <li><a href=""/devices#ps3"" token=""bottombar"">PlayStation 3</a></li>
            <li><a href=""/devices#ios"" token=""bottombar"">Apple iOS</a></li>
            <li><a href=""/devices#android"" token=""bottombar"">Android</a></li>
            <li><a href=""/devices#winphone"" token=""bottombar"">Windows Phone</a></li>
            <li><a href=""/devices#roku"" token=""bottombar"">Roku Box</a></li>
            <li><a href=""/devices#googletv"" token=""bottombar"">Google TV</a></li>
                                    <li style=""margin-top:10px;"">
              <a href=""https://twitter.com/Crunchyroll"" class=""twitter-follow-button"" data-show-count=""false"">Follow @Crunchyroll</a>
<script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0];if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=""//platform.twitter.com/widgets.js"";fjs.parentNode.insertBefore(js,fjs);}}(document,""script"",""twitter-wjs"");</script>
            </li>
                        <li>
              <iframe src=""http://www.facebook.com/plugins/like.php?app_id=195153900534878&amp;href=http%3A%2F%2Fwww.facebook.com%2FCrunchyroll&amp;send=false&amp;layout=button_count&amp;width=115&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font&amp;height=21"" style=""border:none; overflow:hidden; width:115px; height:21px;display:inline;"" allowtransparency=""true"" scrolling=""no"" frameborder=""0""></iframe>
            </li>
                      </ul>
        </td>
        
        <td class=""footer-column"">
          <h6>Premium Memberships</h6>
          <ul>
            <li><a href=""/freetrial?from=bottombar"">Two-Week Free Trial</a></li>
            <li><a href=""/premium_comparison?src=bottombar"">Compare Plans</a></li>
            <li><a href=""/premium_membership_gift?src=bottombar"">Gift Memberships</a></li>
          </ul>
          
          <br/>

          <h6>Language                                    <img id=""footer_country_flag"" src=""http://static.ak.crunchyroll.com/i/country_flags/us.gif""
                 alt=""United States of America""
                 title=""Your detected location is United States of America."" />
                      </h6>
          <ul class=""footer-language"">
                        <li><a href=""#"" onclick=""return Localization.SetLang(&quot;enUS&quot;);"" class=""selected"">English (US)</a></li>
                        <li><a href=""#"" onclick=""return Localization.SetLang(&quot;enGB&quot;);"" class="""">English (UK)</a></li>
                        <li><a href=""#"" onclick=""return Localization.SetLang(&quot;esLA&quot;);"" class="""">Español</a></li>
                        <li><a href=""#"" onclick=""return Localization.SetLang(&quot;jaJP&quot;);"" class="""">???</a></li>
            
          </ul>

        </td>
        
        <td class=""footer-column"">
          <h6>Support</h6>
          <ul>
            <li><a href=""/help"" token=""bottombar"">Help/FAQ</a></li>
            <li><a href=""/abuse"" token=""bottombar"">Unsubscribe Email</a></li>
            <li><a href=""/staff"" token=""bottombar"">Community Staff</a></li>
            <li><a href=""/help?topic=contact"" token=""bottombar"">Contact Us</a></li>
          </ul>
        </td>
    
        <td class=""footer-column"">
          <h6>Crunchyroll</h6>
          <ul>
            <li><a href=""/about"" token=""bottombar"">About</a></li>
            <li><a href=""/jobs"" token=""bottombar"">Jobs</a></li>
            <li><a href=""/advertising"" token=""bottombar"">Advertising</a></li>
            <li><a href=""/dmca_policy"" token=""bottombar"">Copyright Policy</a></li>
            <li><a href=""/tos"" token=""bottombar"">Terms of Service</a></li>
            <li><a href=""/privacy"" token=""bottombar"">Privacy Policy</a></li>
          </ul>
        </td>

      </tr>
    </table>
  </div>
  </div>

              </div>
    </div>

        <script type=""text/javascript"" src=""http://adserver.adtechus.com/addyn/3.0/5290/1285661/0/509/ADTECH;loc=100;target=_blank;grp=68297359;misc=68297359""></script>
<noscript><a href=""http://adserver.adtechus.com/adlink/3.0/5290/1285661/0/16/ADTECH;loc=300;grp=68297359"" target=""_blank""><img src=""http://adserver.adtechus.com/adserv/3.0/5290/1285661/0/16/ADTECH;loc=300;grp=68297359"" border=""0"" width=""1"" height=""1""/></a></noscript>            
    <script type=""text/javascript"">
      $(document).ready(function(e) {
        $('a').each(function() {
          trackToken(this);
        });
        CharacterCounter.Init();
              });

      $(window).bind('beforeunload',function () {
        if (Page.warnOnLeavingPageMessage) {
          return Page.warnOnLeavingPageMessage;
        }
      });

      gapi.plusone.go();
    </script>
    <script type=""text/javascript"">
  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-579606-1']);
  _gaq.push(['_setDomainName', 'crunchyroll.com']);
  _gaq.push(['_setAllowLinker', true]);
  _gaq.push([""_setCustomVar"",1,""User Type"",""not-registered"",1]);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();
</script>

<script type=""text/javascript"">
  var _qevents = _qevents || [];

  (function() {
  var elem = document.createElement('script');
  elem.src = (document.location.protocol == ""https:"" ? ""https://secure"" : ""http://edge"") + "".quantserve.com/quant.js"";
  elem.async = true;
  elem.type = ""text/javascript"";
  var scpt = document.getElementsByTagName('script')[0];
  scpt.parentNode.insertBefore(elem, scpt);
  })();

  _qevents.push({
    qacct:""p-2c9Xi7EeE3Hx2""
  });
</script>
<noscript>
  <div style=""display:none;"">
    <img src=""//pixel.quantserve.com/pixel/p-2c9Xi7EeE3Hx2.gif"" border=""0"" height=""1"" width=""1"" alt=""Quantcast""/>
  </div>
</noscript>



<!-- Begin comScore Tag -->
<script type=""text/javascript"">
  var _comscore = _comscore || [];
  _comscore.push({ c1: ""2"", c2: ""7021617"" });
  (function() {
    var s = document.createElement(""script""), el = document.getElementsByTagName(""script"")[0]; s.async = true;
    s.src = (document.location.protocol == ""https:"" ? ""https://sb"" : ""http://b"") + "".scorecardresearch.com/beacon.js"";
    el.parentNode.insertBefore(s, el);
  })();
</script>
<noscript>
  <img src=""//sb.scorecardresearch.com/p?c1=2&amp;c2=7021617&amp;cv=2.0&amp;cj=1"" />
</noscript>
<!-- End comScore Tag -->

  </body>
  <!--  pylon20 : crunchyroll-20120824T1103-r54313-live-product190 -->
</html>

";
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