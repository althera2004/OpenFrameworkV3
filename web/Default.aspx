<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

    <!DOCTYPE html>
    <html lang="<%=this.InstanceLanguage %>">
    <head>
        <meta charset="utf-8" />
        <title><%=this.InstanceName %> - Open Framework</title>
        <link rel="icon" href="/favicon.ico" type="image/x-icon" />
        <!--[if !IE]> -->
        <!-- script type="text/javascript" src="/assets/js/jquery-2.1.4.min.js"></script -->
        <!-- <![endif]-->

        <!--[if IE]>
            <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></!-->
        <!--[endif]-->

        <meta name="description" content="User login page" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link type="text/css" rel="stylesheet" href="/assets/css/bootstrap.min.css" />
        <link type="text/css" rel="stylesheet" href="/assets/font-awesome/4.5.0/css/font-awesome.min.css" />
        <link type="text/css" rel="stylesheet" href="/assets/css/fonts.googleapis.com.css" />
        <link type="text/css" rel="stylesheet" href="/assets/css/ace.min.css" />
        <link type="text/css" rel="stylesheet" href="/assets/css/ace-skins.min.css" />
        <link type="text/css" rel="stylesheet" href="/assets/css/ace-rtl.min.css" />
        <link type="text/css" rel="stylesheet" href="/assets/css/jquery.virtualKeyboard.css" />
        <link type="text/css" rel="stylesheet" href="/assets/css/flag-icon.min.css" />

        <!--[if IE 7]>
            <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
        <![endif]-->

        <style type="text/css">
            @media only screen and (min-width:481px) {
                .only-480 {
                    display: none !important;
                }
            }

            .widget-main {
                -webkit-box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);
                -moz-box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);
                box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);
            }

            a:hover {
                text-decoration: none;
                font-weight: bold;
            }

            .btn {
                border-radius: 5px;
            }

            input {
                height: 34px !important;
            }

            .containerBGallery {
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                width: 100%;
                height: 100%;
                position: fixed;
            }

                .containerBGallery .slide {
                    z-index: 1;
                    position: absolute;
                    width: 100%;
                    top: 0;
                    left: 0;
                    height: 100%;
                    transition: opacity 1s ease-in-out;
                    background-position: center center;
                    background-repeat: no-repeat;
                    background-size: cover;
                    opacity: 0;
                }

                .containerBGallery .show {
                    opacity: 1 !important;
                }

            body {
                -webkit-touch-callout: none;
                -webkit-user-select: none;
                -khtml-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
                height: 99%;
                width: 99%;
                background-color: #fff;
                overflow:hidden;
            }

            .dd-select, .dd-container, dd-options dd-click-off-close{width:150px!important}
            .dd-desc{display:none !important}
            .dd-selected, .dd-option{padding:2px !important; height:30px;}
            .dd-option-image,.dd-selected-image{margin-top:3px !important;margin-left:2px !important;margin-top:6px!important;}
            .dd-selected-text, .dd-option-text {margin-top:3px!important; cursor:pointer;}

            #CmbPais a { color:#000; text-shadow: 1px 1px 1px #fff;}
            .dd-options {
                border:none!important;
            }

            .dd-option {}

            .dd-selected-image, .dd-option-image {
                width: 20px;
            }
        </style>

        <!--[if lte IE 8]>
              <link rel="stylesheet" href="/assets/css/ace-ie.min.css" />
        <![endif]-->

        <!--[if lt IE 9]>
            <script src="/assets/js/html5shiv.js"></script>
            <script src="/assets/js/respond.min.js"></script>
        <![endif]-->
		<script type="text/javascript" src="/assets/js/purify.js"></script>
        <script type="text/javascript" src="/assets/js/jquery-2.1.4.min.js"></script>
        <script type="text/javascript" src="/js/base64.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.virtualKeyboard.js"></script>
        <script type="text/javascript" src="/js/Login.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/assets/js/jquery.ddslick.min.js"></script>
        <script type="text/javascript">
            localStorage.clear();
            var Instance = <%=this.Instance.Config.JsonData %>;
            localStorage.setItem("Instance", JSON.stringify(Instance));
            var test = "<%=this.ServerTest%>";
            var ddData = [{ "text": "Català", "value": "45", "selected": true, "description": "Català", "imageSrc": "assets/flags/45.png" }, { "text": "Español", "value": "1", "selected": false, "description": "Español", "imageSrc": "assets/flags/1.png" }, { "text": "Français", "value": "7", "selected": false, "description": "Français", "imageSrc": "assets/flags/7.png" }, { "text": "English", "value": "12", "selected": false, "description": "English", "imageSrc": "assets/flags/12.png" }];
            var landPage = "<%=this.LandPage %>";
            var language = Instance.DefaultLanguage;
            var ip = "<%=this.IP %>";
            var multiCompany = <%= this.MultiCompany %>;
            var instanceName = "<%=this.InstanceName %>";
            var MFA = "<%=this.MFA%>";
            var Dictionary =
                {
                    "es-es": {
                        "Title": "Acceso a la aplicación",
                        "Btn": "Acceder",
                        "BtnPublicAccess": "Aceso público",
                        "BtnCorporativeOk": "Continuar",
                        "BtnCorporativeCancel": "Cancelar",
                        "MsgCorporative": "Accederá con un usuario corporativo.<br />Por favor, indique su nombre para indentificarse.",
                        "Cnn": "Conectando...",
                        "UserRequired": "El usuario es obligatorio",
                        "PasswordInvalid": "El mail y/o la contraseña no son válidos",
                        "Remember": "Recuperar contraseña"
                    },
                    "ca-es": {
                        "Title": "Accès a l'aplicació",
                        "Btn": "Accedir",
                        "BtnPublicAccess": "Accés públic",
                        "BtnCorporativeOk": "Continuar",
                        "BtnCorporativeCancel": "Cancel·lar",
                        "MsgCorporative": "Accedirà amb un usuari corporatiu.<br />Si us plau, indiqui el seu nom par a indentificar-se.",
                        "Cnn": "Conectant...",
                        "UserRequired": "L'usuari és obligatori",
                        "PasswordInvalid": "El mail i/o la paraula de pas no són vàlids",
                        "Remember": "Recuperar paraula de pas"
                    }
            };

            window.onload = function () {
                $("#PageTitle").html(Dictionary[language].Title);
                $("#BtnLogin").html(Dictionary[language].Btn);
                $("#ErrorSpan").html(Dictionary[language].PasswordInvalid);
                $("#BtnCorporativeOk").html(Dictionary[language].BtnCorporativeOk);
                $("#BtnCorporativeCancel").html(Dictionary[language].BtnCorporativeCancel);
                $("#CorporativeMessage").html(Dictionary[language].MsgCorporative);
            };

            function cycleBackgrounds() {
                var index = 0; 
                $imageEls = $(".containerBGallery .slide"); 
                if ($imageEls.length > 1) {
                    setInterval(function () {
                        index = index + 1 < $imageEls.length ? index + 1 : 0;
                        $imageEls.eq(index).addClass("show");
                        $imageEls.eq(index - 1).removeClass("show");
                    }, 4000);
                }
            };

            $(function () {
                cycleBackgrounds();
            });
        </script>
    </head>

    <body class="login-layout" oncontextmenu="return false;">    
        <div class="containerBGallery"> 
            <%=this.BK%>
        </div>
        <!--<div class="fullscreen-bg">
            <video loop muted autoplay poster="http://sandbox.thewikies.com/vfe-generator/images/big-buck-bunny_poster.jpg" class="fullscreen-bg__video">
                <source src="https://ak8.picdn.net/shutterstock/videos/18843923/preview/stock-footage--k-students-raising-their-hands-in-a-classroom-to-ask-a-question-slow-motion.mp4" type="video/mp4">
                <source src="https://ak8.picdn.net/shutterstock/shutterstock/videos/18843923/preview/stock-footage--k-students-raising-their-hands-in-a-classroom-to-ask-a-question-slow-motion.webm" type="video/webm">
                <source src="https://ak8.picdn.net/shutterstock/videos/18843923/preview/stock-footage--k-students-raising-their-hands-in-a-classroom-to-ask-a-question-slow-motion.ogv" type="video/ogg">
            </video>
        </div>-->
        <div id="UnanavilableDiv" style="display:none;">
            <div style="position:absolute;width:500px;top:30%;left:50%;margin-left:-250px;background-color:#fefefe;font-size:18px;text-align:center;border:1px solid #333;">
                
                                            <%=this.Logo %>
                                            <div class="space-6"></div>
                Temporalmente fuera de servicio por mantenimiento
            </div>
        </div>
        <div style="position:absolute; top:20px; right:20px">
           <select id="CmbPais" onchange="alert(this.value);" class="col-xs-12 col-sm-12"></select>
        </div>
        <div class="main-container" style="display:_none;margin-top:150px;height:100%;">
            <div class="main-content">
                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <div class="position-relative">
                                <div id="login-box" class="login-box visible widget-box no-border" style="z-index: 99; background-color: transparent;">
                                    <div class="widget-body">
                                        <div class="widget-main" style="text-align: center;">
                                            <h4 class="header black lighter bigger" id="PageTitle" style="font-weight: bold;">&nbsp;</h4>
                                            <img src="<%=this.Logo %>" alt="<%=this.InstanceName %>" title="<%=this.InstanceName %>" style="max-width:100%;" />
                                            <div class="space-6"></div>
                                            <form id="LoginForm" action="InitSession.aspx" method="post">
                                                <div style="display: none;">
                                                    <input type="text" name="UserId" id="UserId" />
                                                    <input type="text" name="CompanyId" id="CompanyId" />
                                                    <input type="text" name="Password" id="Password" />
                                                    <input type="text" name="LandPage" id="LandPage" />
                                                </div>
                                                <fieldset>
                                                    <div class="block clearfix">
                                                        <span class="block input-icon input-icon-right">
                                                            <input type="text" class="form-control TxtUserName" placeholder="Email" id="<%=this.AntiCache %>" value="info@openframework.cat" spellcheck="false" autocomplete="off" />
                                                        </span>
                                                    </div>
                                                    <div class="block clearfix" style="margin-top:8px;">
                                                        <div class="input-group">
                                                            <input type="password" class="form-control TxtPassword" placeholder="Password" id="p<%=this.AntiCache %>" value="P@ssw0rd" spellcheck="false" autocomplete="off" />
                                                            <span class="input-group-addon" onclick="PassKeyBoardToggle();">
                                                                <i class="menu-icon fa fa-keyboard"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="space"></div>
                                                    <div class="clearfix">
                                                        <button type="button" class="width-35 pull-right btn btn-sm btn-primary" id="BtnLogin"></button>
                                                        <button type="button" class="width-35 pull-right btn btn-sm btn-primary" id="BtnLoginQR" style="margin-right:8px;" disabled="disabled" title="Opció no activada"><i class="fa fa-qrcode"></i>&nbsp;QR</button>
                                                        <span id="LoadingMessage" style="font-size: 16px; display: none;"><i class="fas fa-spinner fa-spin"></i>&nbsp;Accediendo...</span>
                                                    </div>
                                                    <div class="space-4"></div>
                                                    <h4>
                                                        <span id="ErrorSpan" style="color: #f00; display: none;"></span>
                                                    </h4>
                                                    <div id="Remember" style="display: none;">
                                                        <hr />
                                                        <button type="button" class="pull-right btn btn-sm btn-success col-xs-12" id="BtnRemember">dfdfdf</button>
                                                    </div>
                                                    <div id="Corporative" style="display: none;">
                                                        <div id="CorporativeMessage" style="text-align: left; padding: 4px; border: 1px solid #ddd; background-color: #f0f0f0;"></div>
                                                        <label class="block clearfix">
                                                            <span class="block input-icon input-icon-right">
                                                                <input type="text" class="form-control TxtName" placeholder="Nombre, mínimo 3 caracteres" id="n<%=this.AntiCache %>" value="" spellcheck="false" autocomplete="off" />
                                                            </span>
                                                        </label>
                                                        <button type="button" class="pull-right btn btn-sm btn-danger col-xs-6" id="BtnCorporativeCancel" onclick="CoporativeLayoutHide();"></button>
                                                        <button type="button" class="pull-right btn btn-sm btn-success col-xs-6" id="BtnCorporativeOk" onclick="CorporativeOk();"></button>
                                                    </div>
                                                </fieldset>
                                            </form>
                                            <hr />
                                            <div style="text-align:left;color:#aaa;">
                                                IP: <span id="IpSpan"></span>
                                                <br />
                                                <span style="color:#999;">Open</span><strong style="color:#55779f">Framework</strong><%=this.FrameworkVersion %>
                                            </div>
                                        </div>
                                    </div>
                                    <% if (!string.IsNullOrEmpty(this.MFA)) { %>
                                            <%= this.MFA %>
                                    <% } %>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- /.main-container -->
        <script type="text/javascript">
            if ("ontouchend" in document) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
        </script>
    </body>
</html>