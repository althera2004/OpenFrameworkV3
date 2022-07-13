<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Services.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Company.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row">    
        
        <div class="col-lg-12">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    <div class="panel-tools">
                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                    </div>
                    Subscripción
                </div>
                <div class="panel-body">
                    Servicio activo desde <strong><span id="SubscriptionStart"></span></strong> hasta <strong><span id="SubscriptionEnd"></span></strong>, quedan <span id="SubscriptionRemains"></span> hasta su finalización.
                </div>
                <div class="panel-footer">
                    <label>Acciones:</label>&nbsp;
                    <button class="btn btn-xs btn-success">Renovar</button>
                    <button class="btn btn-xs btn-info" onclick="GoEncryptedPage('/Admin/Company/Plans.aspx');">Modificar servicios y productos</button>
                </div>
            </div>
        </div>

        <div class="col-lg-6">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    <div class="panel-tools">
                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                    </div>
                    Servicios y productos
                </div>
                <div class="panel-body">
                    <table>
                        <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i id="Check_S" class="FeatureIcon fa"></i></td>
                            <td><label>Anotaciones</label></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom:8px;">Permite añadir postits a los elementos del programa </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i id="Check_CA"  class="FeatureIcon fa"></i></td>
                            <td><label>Alertas personalizadas</label></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom:8px;">Permite activar y desactivar las alertas del sistema y modificar su periodificación</td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i id="Check_LU"  class="FeatureIcon fa"></i></td>
                            <td><label>Bloqueo de usuarios</label></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom:8px;">Permite bloquear usuarios con tareas pendientes mientras éstas no se realicen</td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i id="Check_F"  class="FeatureIcon fa"></i></td>
                            <td><label>Seguimientos</label></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom:8px;">Permite el seguimiento de cambios en los datos seleccionados</td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i id="Check_Dw"  class="FeatureIcon fa"></i></td>
                            <td><label>Descarga de datos</label></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom:8px;">Ofrece la posbilidad de descarga la información de un ítem en formato XML ó JSON y todos los documentos asociados</td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>        
        <div class="col-lg-6">
                <div class="hpanel hyellow">
                <div class="panel-heading hbuilt">
                    <div class="panel-tools">
                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                    </div>
                    Servicios y productos personalizados
                </div>
                <div class="panel-body">
                    <table>
                        <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i class="FeatureIcon fa"></i></td>
                            <td><label>Personalización de presentación</label></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom:8px;">Permite el uso de plantillas propias para la generación de informes, facturas, emails, etc...</td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i class="FeatureIcon fa"></i></td>
                            <td><label>Instalación en servidor propio</label></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom:8px;">La aplicación se instala en un servidor propio aportado por el cliente</td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i class="FeatureIcon fa"></i></td>
                            <td><label>Asistencia técnica directa</label></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom:8px;">Acceso directo a un asiste técnico para gestión de incidencias y atención al usuario </td>
                        </tr>
                    </table>
                </div>
                    </div>
        </div>
        <div class="col-lg-12">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    <div class="panel-tools">
                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                    </div>
                    Términos, contrato y condiciones
                </div>
                <div class="panel-body">
                    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingZero">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseZero" aria-expanded="true" aria-controls="collapseOne">
                                        Contrato de subscripción
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseZero" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingZero">
                                <div class="panel-body">
                                    <div class="col-xs-6">
                                        sdsf df sdfsdf                                    
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <label>Acciones:</label>&nbsp;
                                    <button class="btn btn-info btn-xs">Descargar en PDF</button>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingOne">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                        Términos y condiciones
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                <div class="panel-body">
                                    <div class="col-xs-6">
                                        sdsf df sdfsdf                                    
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <label>Acciones:</label>&nbsp;
                                    <button class="btn btn-info btn-xs">Descargar en PDF</button>
                                </div>
                            </div>                            
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingTwo">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="true" aria-controls="collapseOne">
                                        Política de cookies
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                <div class="panel-footer">
                                    <label>Accions:</label>&nbsp;
                                    <input type="radio" name="RBPC" checked="checked" onclick="$('#PCCat').show();$('#PCCas').hide();" /> Veure en català
                                    &nbsp;
                                    <input type="radio" name="RBPC" onclick="$('#PCCas').show();$('#PCCat').hide();" /> Ver en castellano
                                </div>
                                <div class="panel-body">
                                    
                                    <div id="PCCat">
                                    <h1 class="has-text-align-center holded-header__heading"><strong>Política de cookies</strong></h1>
                                    <p class="has-text-align-center">Darrea actualització: 1 d'octubre de 2017</p>
                                    <div class="wp-block-group holded-section">
                                        <div class="wp-block-group__inner-container">
                                            <div class="wp-block-group__inner-container">
                                                <p><strong>Introducció</strong></p>
                                                <p class="mb-30">A <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, igual que a la majoria de portals a Internet, per a mejorar la experiencia del usuario utilizamos cookies en nuestras páginas web <em>openframework.es</em> y <em>openframework.cat</em> (en adelante, el “Sitio Web”).<br>
                                                    <br>
                                                    A la seva primera visita al nostre Web se li informa de l'existència de cookies i de la present política de cookies. En futuras visitas puede consultar nuestra política en cualquier momento en la parte inferior del Sitio Web “Política de Cookies”. Amb el seu registre al Sitio Web i/o la mera navegació estè consentit l'instalació de les cookies de les que l'informem a la present “Política de Cookies” (a menys que hagi modificat la configuració del seu navegador per a rebutjar cookies).<br />
                                                    <br>
                                                    A continuación encontrará información sobre qué son las “cookies”, qué tipo de cookies utiliza este portal, cómo puede desactivar las cookies en su navegador, cómo desactivar específicamente la instalación de cookies de terceros. Si no encuentra la información específica que usted está buscando, por favor diríjase a <a href="mailto:info@openframework.cat"><em>info@openframework.cat</em></a></p>
                                                <p><strong>¿Qué son y para qué se utilizan las cookies?</strong></p>
                                                <p>Una cookie es un fichero que se descarga en el dispositivo del Usuario al acceder a determinados contenidos por páginas web, medios electrónicos y/o aplicaciones. Las cookies permiten a una aplicación o página web, entre otras cosas, almacenar y recuperar información sobre los hábitos de navegación de un Usuario o de su equipo y, dependiendo de la información que contengan y de la forma en que utilice su dispositivo, pueden utilizarse para reconocer al Usuario.<br>
                                                    <br>
                                                    Las cookies son seguras en la medida en que sólo pueden almacenar la información utilizada por el navegador, la información que el usuario ha introducido en el navegador o la que se incluye en la solicitud de página.<br>
                                                    <br>
                                                    Algunas “cookies” son estrictamente necesarias para el correcto funcionamiento de un sitio web y/o aplicación y no pueden contener virus ni dañar tu dispositivo.<br>
                                                    <br>
                                                    Otras sirven para propósitos diversos, como favorecer la navegación entre páginas y/o aplicaciones, almacenar el idioma de preferencia del usuario, permitir recordar las preferencias como usuario, o detectar si lo has visitado previamente.<br>
                                                    <br>
                                                    Las cookies son esenciales para el funcionamiento de internet, aportando innumerables ventajas en la prestación de servicios interactivos, facilitando la navegación e interacción. Las cookies no pueden dañar el equipo y que estén activadas ayuda a identificar y resolver los potenciales errores.</p>
                                                <p><strong>¿Qué tipología de cookies se utilizan en <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>?</strong></p>
                                                <p>2.1. 2.1. En función de la entidad que las gestiona, las cookies que se utilizan se pueden clasificar en:</p>
                                                <ul>
                                                    <li>(i) Cookies propias: son aquellas Cookies que son enviadas a tu dispositivo y gestionadas exclusivamente por nosotros para el mejor funcionamiento de <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>. La información que se recoge se emplea para mejorar la calidad de nuestro servicio y tu experiencia como usuario.</li>
                                                    <li>(ii) Cookies de terceros: son cookies que deposita un servidor de otro dominio, con la autorización del sitio que estás visitando (por ejemplo, al pulsar botones de redes sociales o ver vídeos alojados en otro sitio web o aplicación). No podemos acceder a los datos almacenados en las cookies de otros sitios web o aplicaciones cuando navegues en los citados sitios web o aplicaciones. En el caso de que las cookies sean instaladas desde un equipo o dominio gestionado por <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> pero la información que se recoja mediante éstas sea gestionada por un tercero, no pueden ser consideradas como cookies propias.</li>
                                                </ul>
                                                <p>2.2. 2.2. En función del plazo de tiempo que permanecen activadas, puede diferenciarse entre las siguientes cookies:</p>
                                                <ul>
                                                    <li>(i) Cookies de sesión: son archivos temporales que permanecen en el histórico de cookies del navegador hasta que se abandona el Sitio Web o aplicaciones, por lo no se registran en el disco duro del ordenador o dispositivo móvil del usuario. Los datos obtenidos por medio de estas cookies sirven para analizar pautas de tráfico y comunicación de datos. Permiten proporcionar una mejor experiencia para mejorar el contenido y facilitar su uso.</li>
                                                    <li>(ii) Cookies persistentes: son almacenadas en el disco duro y el Sitio Web y aplicaciones las leen cada vez que se realiza una nueva visita. A pesar de su nombre, una cookie persistente posee una fecha de expiración determinada. La cookie dejará de funcionar después de esa fecha. Se utilizan, generalmente, para facilitar los diferentes servicios que ofrecen las páginas web y aplicaciones.</li>
                                                </ul>
                                                <p>2.3. 2.3. En función de la finalidad de la cookie:</p>
                                                <ul>
                                                    <li>Cookies de <strong>registro</strong>: Cuando el usuario se registra en <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, se generan cookies que le identifican como usuario registrado e indican cuándo se ha identificado en el portal.<br>
                                                        Estas cookies son utilizadas para identificar la cuenta de usuario y sus servicios asociados. Estas cookies se mantienen mientras el usuario no abandone la cuenta, cierre el navegador o apague el dispositivo.</li>
                                                    <li><strong>Cookies analíticas</strong>: las cookies pueden ser utilizadas en combinación con datos analíticos para identificar de manera individual las preferencias del usuario en <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>.</li>
                                                    <li><strong>Cookies</strong> de rendimiento: este tipo de cookies conserva las preferencias del usuario para ciertas herramientas o servicios para que no tenga que configurarlos cada vez que visita <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> y, en algunos casos, pueden ser aportadas por terceros. Algunos ejemplos son: volumen de los reproductores audiovisuales, preferencias de ordenación de artículos o velocidades de reproducción de vídeo compatibles.</li>
                                                    <li><strong>Cookies publicitarias</strong>Son aquéllas que permiten la gestión, de la forma más eficaz posible, de los espacios publicitarios que, en su caso, el editor haya incluido en una página web, aplicación o plataforma desde la que presta el servicio solicitado en base a criterios como el contenido editado o la frecuencia en la que se muestran los anuncios.</li>
                                                    <li>Cookies de <strong>publicidad de comportamiento</strong>: Son aquéllas que permiten la gestión, de la forma más eficaz posible, de los espacios publicitarios que, en su caso, el editor haya incluido en una página web, aplicación o plataforma desde la que presta el servicio solicitado. Estas cookies almacenan información del comportamiento de los usuarios obtenida a través de la observación continuada de sus hábitos de navegación, lo que permite desarrollar un perfil específico para mostrar publicidad en función del mismo.</li>
                                                    <li>Cookies de <strong>geolocalización</strong>: estas cookies son usadas por programas que intentan localizar geográficamente la situación del ordenador, smartphone, tableta o televisión conectada, para de manera totalmente anónima ofrecerle contenidos y servicios más adecuados.</li>
                                                    <li><strong>Otras cookies de terceros</strong>: en algunas de nuestras páginas se pueden instalar cookies de terceros que permitan gestionar y mejorar los servicios que éstos ofrecen. Un ejemplo de este uso son los enlaces a las redes sociales que permiten compartir nuestros contenidos.</li>
                                                </ul>
                                                <p><strong>¿Cómo puede el usuario bloquear o eliminar las cookies?</strong></p>
                                                <p>El usuario puede permitir, bloquear o eliminar las cookies instaladas en su equipo mediante la configuración de las opciones de su navegador de internet. En el caso en que las bloquee, es posible que ciertos servicios que necesitan su uso no estén disponibles para el usuario.<br>
                                                    A continuación ofrecemos al usuario enlaces en los que encontrará información sobre cómo puede activar sus preferencias en los principales navegadores:</p>
                                                <ul>
                                                    <li><strong>Internet Explorer</strong>:<encoded_tag_closed> </encoded_tag_closed>
                                                        Herramientas<encoded_tag_closed> </encoded_tag_closed>
                                                        –<encoded_tag_closed> </encoded_tag_closed>
                                                        Opciones de Internet – Privacidad – Configuración.<br>
                                                        Para más información, puede consultar el soporte de Microsoft o la Ayuda del navegador.</li>
                                                    <li><strong>Firefox</strong>:<encoded_tag_closed> </encoded_tag_closed>
                                                        <encoded_tag_closed></encoded_tag_closed>
                                                        Opciones<encoded_tag_closed> </encoded_tag_closed>
                                                        <encoded_tag_closed></encoded_tag_closed>
                                                        de herramientas Historial de privacidad Configuración personalizada.<br>
                                                        Para más información, puede consultar el soporte de Mozilla o la Ayuda del navegador.</li>
                                                    <li><strong>Chrome</strong>:<encoded_tag_closed> </encoded_tag_closed>
                                                        Configuración<encoded_tag_closed> </encoded_tag_closed>
                                                        Mostrar<encoded_tag_closed> </encoded_tag_closed>
                                                        opciones avanzadas Configuración de contenido de privacidad.<br>
                                                        Para más información, puede consultar el soporte de Google o la Ayuda del navegador.</li>
                                                    <li><strong>Safari</strong>:<encoded_tag_closed> </encoded_tag_closed>
                                                        Preferencias De seguridad</li>
                                                </ul>
                                                <p>Para más información, puede consultar el soporte de Apple o la Ayuda del navegador.<br>
                                                    Finalmente, el usuario puede dirigirse al portal<a href="https://www.youronlinechoices.com/" rel="noopener"><em>Your Online Choices,</em></a> dónde además de encontrar información útil, podrá configurar, proveedor por proveedor, sus preferencias sobre las cookies publicitarias de terceros. Si tiene dudas sobre esta política de cookies, puede contactar con <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> por correo electrónico a <a href="mailto:info@openframework.cat"><em>info@openframework.cat</em></a>.</p>
                                                <p><strong>Consentimiento</strong></p>
                                                <p>El Usuario puede revocar su consentimiento previamente otorgado a la instalación de cookies en cualquier momento eliminando las cookies instaladas en su equipo mediante la configuración de las opciones del navegador, tal como se indica en el apartado “revocación y eliminación de cookies”. No obstante, ello puede impactar en las funcionalidades del Sitio Web o de las aplicaciones que utiliza el Usuario, haciendo la experiencia del Usuario menos satisfactoria.</p>
                                            </div>
                                        </div>
                                    </div>
                                        </div>
                                    <div id="PCCas" style="display:none;">
                                    <h1 class="has-text-align-center holded-header__heading"><strong>Política de cookies</strong></h1>
                                    <p class="has-text-align-center">última actualización: 1 de octubre de 2017</p>
                                    <div class="wp-block-group holded-section">
                                        <div class="wp-block-group__inner-container">
                                            <div class="wp-block-group__inner-container">
                                                <p><strong>Introducción</strong></p>
                                                <p class="mb-30">En <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, al igual que la mayoría de portales en Internet, para mejorar la experiencia del usuario utilizamos cookies en nuestras páginas web <em>openframework.es</em> y <em>openframework.cat</em> (en adelante, el “Sitio Web”).<br>
                                                    <br>
                                                    En su primera visita a nuestro Sitio Web se le informa de la existencia de cookies y de la presente política de cookies. En futuras visitas puede consultar nuestra política en cualquier momento en la parte inferior del Sitio Web “Política de Cookies”. Con su registro en el Sitio Web y/o la mera navegación está consintiendo la instalación de las cookies de las que le informamos en la presente “Política de Cookies” (salvo que haya modificado la configuración de su navegador para rechazar cookies).<br>
                                                    <br>
                                                    A continuación encontrará información sobre qué son las “cookies”, qué tipo de cookies utiliza este portal, cómo puede desactivar las cookies en su navegador, cómo desactivar específicamente la instalación de cookies de terceros. Si no encuentra la información específica que usted está buscando, por favor diríjase a <a href="mailto:info@openframework.cat"><em>info@openframework.cat</em></a></p>
                                                <p><strong>¿Qué son y para qué se utilizan las cookies?</strong></p>
                                                <p>Una cookie es un fichero que se descarga en el dispositivo del Usuario al acceder a determinados contenidos por páginas web, medios electrónicos y/o aplicaciones. Las cookies permiten a una aplicación o página web, entre otras cosas, almacenar y recuperar información sobre los hábitos de navegación de un Usuario o de su equipo y, dependiendo de la información que contengan y de la forma en que utilice su dispositivo, pueden utilizarse para reconocer al Usuario.<br>
                                                    <br>
                                                    Las cookies son seguras en la medida en que sólo pueden almacenar la información utilizada por el navegador, la información que el usuario ha introducido en el navegador o la que se incluye en la solicitud de página.<br>
                                                    <br>
                                                    Algunas “cookies” son estrictamente necesarias para el correcto funcionamiento de un sitio web y/o aplicación y no pueden contener virus ni dañar tu dispositivo.<br>
                                                    <br>
                                                    Otras sirven para propósitos diversos, como favorecer la navegación entre páginas y/o aplicaciones, almacenar el idioma de preferencia del usuario, permitir recordar las preferencias como usuario, o detectar si lo has visitado previamente.<br>
                                                    <br>
                                                    Las cookies son esenciales para el funcionamiento de internet, aportando innumerables ventajas en la prestación de servicios interactivos, facilitando la navegación e interacción. Las cookies no pueden dañar el equipo y que estén activadas ayuda a identificar y resolver los potenciales errores.</p>
                                                <p><strong>¿Qué tipología de cookies se utilizan en <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>?</strong></p>
                                                <p>2.1. 2.1. En función de la entidad que las gestiona, las cookies que se utilizan se pueden clasificar en:</p>
                                                <ul>
                                                    <li>(i) Cookies propias: son aquellas Cookies que son enviadas a tu dispositivo y gestionadas exclusivamente por nosotros para el mejor funcionamiento de <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>. La información que se recoge se emplea para mejorar la calidad de nuestro servicio y tu experiencia como usuario.</li>
                                                    <li>(ii) Cookies de terceros: son cookies que deposita un servidor de otro dominio, con la autorización del sitio que estás visitando (por ejemplo, al pulsar botones de redes sociales o ver vídeos alojados en otro sitio web o aplicación). No podemos acceder a los datos almacenados en las cookies de otros sitios web o aplicaciones cuando navegues en los citados sitios web o aplicaciones. En el caso de que las cookies sean instaladas desde un equipo o dominio gestionado por <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> pero la información que se recoja mediante éstas sea gestionada por un tercero, no pueden ser consideradas como cookies propias.</li>
                                                </ul>
                                                <p>2.2. 2.2. En función del plazo de tiempo que permanecen activadas, puede diferenciarse entre las siguientes cookies:</p>
                                                <ul>
                                                    <li>(i) Cookies de sesión: son archivos temporales que permanecen en el histórico de cookies del navegador hasta que se abandona el Sitio Web o aplicaciones, por lo no se registran en el disco duro del ordenador o dispositivo móvil del usuario. Los datos obtenidos por medio de estas cookies sirven para analizar pautas de tráfico y comunicación de datos. Permiten proporcionar una mejor experiencia para mejorar el contenido y facilitar su uso.</li>
                                                    <li>(ii) Cookies persistentes: son almacenadas en el disco duro y el Sitio Web y aplicaciones las leen cada vez que se realiza una nueva visita. A pesar de su nombre, una cookie persistente posee una fecha de expiración determinada. La cookie dejará de funcionar después de esa fecha. Se utilizan, generalmente, para facilitar los diferentes servicios que ofrecen las páginas web y aplicaciones.</li>
                                                </ul>
                                                <p>2.3. 2.3. En función de la finalidad de la cookie:</p>
                                                <ul>
                                                    <li>Cookies de <strong>registro</strong>: Cuando el usuario se registra en <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, se generan cookies que le identifican como usuario registrado e indican cuándo se ha identificado en el portal.<br>
                                                        Estas cookies son utilizadas para identificar la cuenta de usuario y sus servicios asociados. Estas cookies se mantienen mientras el usuario no abandone la cuenta, cierre el navegador o apague el dispositivo.</li>
                                                    <li><strong>Cookies analíticas</strong>: las cookies pueden ser utilizadas en combinación con datos analíticos para identificar de manera individual las preferencias del usuario en <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>.</li>
                                                    <li><strong>Cookies</strong> de rendimiento: este tipo de cookies conserva las preferencias del usuario para ciertas herramientas o servicios para que no tenga que configurarlos cada vez que visita <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> y, en algunos casos, pueden ser aportadas por terceros. Algunos ejemplos son: volumen de los reproductores audiovisuales, preferencias de ordenación de artículos o velocidades de reproducción de vídeo compatibles.</li>
                                                    <li><strong>Cookies publicitarias</strong>Son aquéllas que permiten la gestión, de la forma más eficaz posible, de los espacios publicitarios que, en su caso, el editor haya incluido en una página web, aplicación o plataforma desde la que presta el servicio solicitado en base a criterios como el contenido editado o la frecuencia en la que se muestran los anuncios.</li>
                                                    <li>Cookies de <strong>publicidad de comportamiento</strong>: Son aquéllas que permiten la gestión, de la forma más eficaz posible, de los espacios publicitarios que, en su caso, el editor haya incluido en una página web, aplicación o plataforma desde la que presta el servicio solicitado. Estas cookies almacenan información del comportamiento de los usuarios obtenida a través de la observación continuada de sus hábitos de navegación, lo que permite desarrollar un perfil específico para mostrar publicidad en función del mismo.</li>
                                                    <li>Cookies de <strong>geolocalización</strong>: estas cookies son usadas por programas que intentan localizar geográficamente la situación del ordenador, smartphone, tableta o televisión conectada, para de manera totalmente anónima ofrecerle contenidos y servicios más adecuados.</li>
                                                    <li><strong>Otras cookies de terceros</strong>: en algunas de nuestras páginas se pueden instalar cookies de terceros que permitan gestionar y mejorar los servicios que éstos ofrecen. Un ejemplo de este uso son los enlaces a las redes sociales que permiten compartir nuestros contenidos.</li>
                                                </ul>
                                                <p><strong>¿Cómo puede el usuario bloquear o eliminar las cookies?</strong></p>
                                                <p>El usuario puede permitir, bloquear o eliminar las cookies instaladas en su equipo mediante la configuración de las opciones de su navegador de internet. En el caso en que las bloquee, es posible que ciertos servicios que necesitan su uso no estén disponibles para el usuario.<br>
                                                    A continuación ofrecemos al usuario enlaces en los que encontrará información sobre cómo puede activar sus preferencias en los principales navegadores:</p>
                                                <ul>
                                                    <li><strong>Internet Explorer</strong>:<encoded_tag_closed> </encoded_tag_closed>
                                                        Herramientas<encoded_tag_closed> </encoded_tag_closed>
                                                        –<encoded_tag_closed> </encoded_tag_closed>
                                                        Opciones de Internet – Privacidad – Configuración.<br>
                                                        Para más información, puede consultar el soporte de Microsoft o la Ayuda del navegador.</li>
                                                    <li><strong>Firefox</strong>:<encoded_tag_closed> </encoded_tag_closed>
                                                        <encoded_tag_closed></encoded_tag_closed>
                                                        Opciones<encoded_tag_closed> </encoded_tag_closed>
                                                        <encoded_tag_closed></encoded_tag_closed>
                                                        de herramientas Historial de privacidad Configuración personalizada.<br>
                                                        Para más información, puede consultar el soporte de Mozilla o la Ayuda del navegador.</li>
                                                    <li><strong>Chrome</strong>:<encoded_tag_closed> </encoded_tag_closed>
                                                        Configuración<encoded_tag_closed> </encoded_tag_closed>
                                                        Mostrar<encoded_tag_closed> </encoded_tag_closed>
                                                        opciones avanzadas Configuración de contenido de privacidad.<br>
                                                        Para más información, puede consultar el soporte de Google o la Ayuda del navegador.</li>
                                                    <li><strong>Safari</strong>:<encoded_tag_closed> </encoded_tag_closed>
                                                        Preferencias De seguridad</li>
                                                </ul>
                                                <p>Para más información, puede consultar el soporte de Apple o la Ayuda del navegador.<br>
                                                    Finalmente, el usuario puede dirigirse al portal<a href="https://www.youronlinechoices.com/" rel="noopener"><em>Your Online Choices,</em></a> dónde además de encontrar información útil, podrá configurar, proveedor por proveedor, sus preferencias sobre las cookies publicitarias de terceros. Si tiene dudas sobre esta política de cookies, puede contactar con <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> por correo electrónico a <a href="mailto:info@openframework.cat"><em>info@openframework.cat</em></a>.</p>
                                                <p><strong>Consentimiento</strong></p>
                                                <p>El Usuario puede revocar su consentimiento previamente otorgado a la instalación de cookies en cualquier momento eliminando las cookies instaladas en su equipo mediante la configuración de las opciones del navegador, tal como se indica en el apartado “revocación y eliminación de cookies”. No obstante, ello puede impactar en las funcionalidades del Sitio Web o de las aplicaciones que utiliza el Usuario, haciendo la experiencia del Usuario menos satisfactoria.</p>
                                            </div>
                                        </div>
                                    </div>
                                        </div>
                                    <p></p>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingThree">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="true" aria-controls="collapseOne">
                                        Política de privacidad
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                <div class="panel-body">
                                    <strong>Introducción</strong><p>La política de privacidad de <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> sigue la legislación internacional y europea (GDPR) sobre protección de datos. Además, <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> pretende informar al usuario a través del presente documento de sus derechos y obligaciones respecto de la privacidad de sus datos, además de explicar el porqué del almacenamiento y uso de los datos.</p><p><strong>Datos recogidos por <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong></strong></p><p><strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> recoge toda la información introducida en la aplicación por el usuario y la almacena en sus servidores propios o en los servidores del proveedor de almacenamiento de <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>. Al tratarse de un servicio Cloud, el almacenamiento en servidores es un requisito para el funcionamiento de la aplicación, es por ello que el usuario acepta tal hecho.<br><br><strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> clasifica los datos en dos categorías: datos de usuario y datos de cuenta. Los datos de usuario comprenden el nombre del usuario y los datos personales, así como fotografías o demás documentación que el usuario decida subir a la aplicación en la parte de información de cuenta y la información de contacto. Los datos de cuenta son aquellos relacionados con la cuenta de negocio creada por el usuario, en cualquiera de sus diferentes formas: Autónomo, Sociedad Civil Personal o Sociedad Limitada, que comprenden datos de facturación, gastos, contactos, trabajadores y productos.<br><br>Por último, <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> recoge datos del dispositivo de conexión a través de cookies. El usuario puede desactivar las cookies en su navegador para que <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> no recoja dicha información en caso que el usuario no quiera que así sea. Dicha información es utilizada para realizar estudios de navegación y acceso a la aplicación, así como del uso de la misma. La información de navegación comprende el tipo de dispositivo y tus características, la ubicación y los tiempos de conexión.</p><p><strong>¿Quién es el responsable del tratamiento de tus datos?</strong></p><p class="mb-30">Identidad: <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong><br>Dirección: Carrer Sant Juame 2, 48830 Torredembarra, España<br>Correo electrónico: info@openframework.cat</p><p><strong>¿Con qué finalidad tratamos tus datos personales?</strong></p><p>En <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> technologies S.L tratamos la información que nos facilitan las personas interesadas con el fin de prestar y/o comercializar los productos y/o servicios ofertados por nuestra firma.<br><br>En particular, en base al interés legítimo regulado en el GDPR, <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> podrá enviar promociones comerciales, newsletter y otra información de interés a todas aquellas personas que se hayan registrado en un webinar u otros actos promovidos por <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>.</p><p><strong>¿Por cuánto tiempo conservaremos tus datos?</strong></p><p class="mb-30">Los datos personales proporcionados se conservarán por el tiempo imprescindible para la prestación del servicio solicitado o comercialización del producto y durante los plazos legalmente establecidos.</p><p><strong>¿Cuál es la legitimación para el tratamiento de tus datos?</strong></p><p class="mb-30">La base legal para el tratamiento de tus datos es a través del consentimiento de la parte afectada, la ejecución de un contrato de servicio y la prestación de respuestas a tus consultas recibidas a través del formulario de contacto o correo electrónico (de acuerdo con los términos y condiciones contenidos en nuestra política de privacidad).</p><p><strong>¿A qué destinatarios se comunicarán tus datos?</strong></p><p class="mb-30">Los datos no se comunicarán a terceros salvo por obligación legal.</p><p><strong>¿Cualés son tus derechos cuando nos facilitas tus datos?</strong></p><p class="mb-30">Cualquier persona tiene derecho a obtener información sobre si en <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> technologies S.L estamos tratando datos personales que les conciernan, o no.<br><br>Las personas interesadas tienen derecho a acceder a sus datos personales, así como a solicitar la rectificación de los datos inexactos o, en su caso, solicitar su supresión cuando, entre otros motivos, los datos ya no sean necesarios para los fines que fueron recogidos.<br><br>En determinadas circunstancias, los interesados podrán solicitar la limitación del tratamiento de sus datos, en cuyo caso únicamente los conservaremos para el ejercicio o la defensa de reclamaciones.<br><br>Podrás ejercitar materialmente tus derechos de la siguiente forma: enviando un correo electrónico a info@openframework.cat identificándote debidamente e indicando de forma expresa el concreto derecho que se quieres ejercer.<br><br>Si has otorgado tu consentimiento para alguna finalidad concreta, tienes derecho a retirar el consentimiento otorgado en cualquier momento, sin que ello afecte a la licitud del tratamiento basado en el consentimiento previo a tu retirada.<br><br>En caso de que sienta vulnerados sus derechos en lo concerniente a la protección de sus datos personales, especialmente cuando no haya obtenido satisfacción en el ejercicio de sus derechos, puede presentar una reclamación ante la Autoridad de Control en materia de Protección de Datos competente a través de su sitio web: www.agpd.es</p><p><strong>¿Cómo hemos obtenido tus datos?</strong></p><p class="mb-30">Todos los datos personales obtenidos por <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> han sido avanzados directamente por el usuario. No tratamos con categorías especiales de datos personales de conformidad con el artículo 9 del Reglamento General de <em>Protección</em> de Datos de la UE (datos personales que revelan el origen racial o étnico, opiniones políticas, creencias religiosas o filosóficas o afiliación sindical).</p><p><strong>Datos compartidos por el usuario</strong></p><p class="mb-30">Muchos de nuestros servicios te permiten compartir información con otros usuarios. Recuerda que, cuando compartes información públicamente, esta puede ser indexada por motores de búsqueda. Nuestros servicios te proporcionan diferentes opciones sobre cómo compartir y eliminar tu contenido. Cómo acceder a tus datos personales y actualizarlos siempre que utilizas nuestros servicios, nuestro objetivo consiste en proporcionarte acceso a tu información personal. Si esa información no es correcta, nos esforzamos para proporcionarte formas de eliminarla o actualizarla rápidamente, a menos que tengamos que mantener esa información por motivos legales o empresariales legítimos. Al actualizar tu información personal, podremos pedirte que verifiques tu identidad para que podamos procesar tu solicitud.<br><br>Podremos rechazar solicitudes que sean más repetitivas de lo razonable, que requieran un esfuerzo técnico desproporcionado (por ejemplo, desarrollar un nuevo sistema o cambiar de forma radical una práctica existente),que pongan en peligro la privacidad de otros usuarios o que no sean nada prácticas (por ejemplo, solicitudes que hagan referencia a información almacenada en sistemas de copia de seguridad). Cuando podamos ofrecerte la posibilidad de acceder a tus datos personales y modificarlos, lo haremos de forma gratuita, salvo que ello requiera un esfuerzo desproporcionado.<br><br>Al prestar nuestros servicios, protegeremos tus datos procurando que no puedan ser eliminados de forma accidental o intencionada. Por este motivo, aunque elimines tus datos de nuestros servicios, es posible que no destruyamos de inmediato las copias residuales almacenadas en nuestros servidores activos ni los datos almacenados en nuestros sistemas de seguridad.</p><p><strong>Acceso y modificación de los datos del usuario</strong></p><p class="mb-30">El usuario podrá modificar en cualquier momento sus datos de usuario o de las cuentas de las que es propietario. <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> no almacena la información modificada, de forma que una vez el usuario modifique o elimine la información, la perderá para siempre dentro de la aplicación.</p><p><strong>Los datos que compartimos</strong></p><p>No compartimos información personal con empresas, organizaciones ni particulares que no tengan relación con <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, a menos que se dé alguna de las siguientes circunstancias:</p><ul class="mb-30"><li>Consentimiento: Compartiremos tus datos personales con empresas, organizaciones o personas físicas ajenas a <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> cuando nos hayas dado tu consentimiento para hacerlo. Tu consentimiento será necesario para compartir datos personales especialmente protegidos.</li><li>Tratamiento externo: Proporcionamos información personal a nuestros afiliados o a otras personas o empresas de confianza para que lleven a cabo su procesamiento por parte de <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, siguiendo nuestras instrucciones y de conformidad con nuestra Política de privacidad, y adoptando otras medidas de seguridad y confidencialidad adecuadas.</li><li>Motivos legales: Compartiremos tus datos personales con empresas, organizaciones o personas físicas ajenas a <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> si consideramos de buena fe que existe una necesidad razonable de acceder a dichos datos o utilizarlos, conservarlos o revelarlos para: cumplir cualquier requisito previsto en la legislación o normativa aplicable o atender cualquier requerimiento de un órgano administrativo o judicial, cumplir lo previsto en las Condiciones de servicio vigentes, incluida la investigación de posibles infracciones, detectar o impedir cualquier fraude o incidencia técnica o de seguridad o hacerles frente de otro modo, proteger los derechos, los bienes o la seguridad de <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, de nuestros usuarios o del público en general en la medida exigida o permitida por la legislación aplicable.</li></ul><p><strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> informa de forma expresa al PARTNER que se reserva el derecho a utilizar los datos de clientes en el entorno de desarrollo, preparación, pruebas y, en general, entornos no productivos de <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, con la finalidad principal de mejorar la plataforma y para que los equipos de desarrollo y customer success puedan identificar errores y anomalías. El PARTNER declara ser consciente de esta práctica y, en lo menester, declara su aceptación</p><p><strong>Seguridad de los datos</strong></p><p>Para <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> la seguridad es lo más importante. Actualmente el servicio de almacenamiento de datos se realiza a través de proveedores especializados con certificados de seguridad y sistemas anti hacking. <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> ha decidido externalizar el almacenamiento para asegurarse que el proveedor cumple los más altos estándares de seguridad, en niveles que <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> no podría ofrecer en servidores de almacenamiento propios.<br><br>Nos esforzamos por proteger a <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> y a nuestros usuarios frente a cualquier modificación, divulgación o destrucción no autorizada de los datos que conservamos frente al acceso no autorizado a los mismos. En particular: Encriptamos muchos de nuestros servicios mediante el protocolo SSL. Revisamos nuestra política en materia de recogida, almacenamiento y tratamiento de datos, incluyendo las medidas de seguridad físicas, para impedir el acceso no autorizado a nuestros sistemas. Limitamos el acceso de los contratistas, los agentes y los empleados de <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> a la información personal que deben procesar para <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> y nos aseguramos de que cumplan las estrictas obligaciones de confidencialidad contractuales y de que estén sujetos a las condiciones disciplinarias pertinentes o al despido si no cumplen dichas obligaciones. Nuestra Política de privacidad se aplica a todos los servicios ofrecidos por <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> y sus filiales, incluidos <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, pero excluye aquellos servicios que estén sujetos a políticas de privacidad independientes que no incorporen la presente Política de privacidad.</p><p><strong>Cuándo se aplica esta Política de privacidad</strong></p><p>Nuestra Política de privacidad se aplica a todos los servicios ofrecidos por <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> y sus filiales, incluidos <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>, pero excluye aquellos servicios que estén sujetos a políticas de privacidad independientes que no incorporen la presente Política de privacidad.</p><p><strong>Cumplimiento de la ley</strong></p><p>En <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> verificamos el cumplimiento de nuestra Política de privacidad de forma regular. Asimismo, nos adherimos a diferentes códigos de autorregulación. En caso de que recibamos una reclamación formal por escrito, nos pondremos en contacto con la persona que la haya formulado para hacer un seguimiento de la misma. Trabajaremos con las autoridades reguladoras competentes, incluyendo las autoridades locales de protección de datos, para resolver cualquier reclamación relacionada con la transferencia de datos de carácter personal que no hayamos podido solucionar directamente con el usuario.</p><p><strong>Modificaciones</strong></p><p>Nuestra Política de privacidad se podrá modificar en cualquier momento. No limitaremos los derechos que te corresponden con arreglo a la presente Política de privacidad sin tu expreso consentimiento. Publicaremos todas las modificaciones de la presente Política de privacidad en esta página y, si son significativas, efectuaremos una notificación más destacada (por ejemplo, te enviaremos una notificación por correo electrónico si la modificación afecta a determinados servicios). Además, archivaremos las versiones anteriores de la presente Política de privacidad para que puedas consultarlas.</p><p><strong>Modificaciones</strong></p><p>Cuando precisemos obtener información por tu parte, siempre te solicitaremos que nos las proporciones voluntariamente prestando tu consentimiento de forma expresa a través de los medios habilitados para ello.<br><br>El tratamiento de los datos recabados a través de los formularios de recogida de datos del sitio web u otras vías, quedará incorporado al Registro de Actividades de Tratamiento del cual es responsable <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong><br><br><strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> trata los datos de forma confidencial y adopta las medidas técnicas y organizativas apropiadas para garantizar el nivel de seguridad adecuado al tratamiento, en cumplimiento de lo requerido por el Reglamento (UE) 2016/679 del Parlamento Europeo y del Consejo de 27 de abril de 2016 y demás normativa aplicable en materia de Protección de Datos.<br><br>No obstante, <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> technologies S.L no puede garantizar la absoluta invulnerabilidad de los sistemas, por tanto, no asume ninguna responsabilidad por los daños y perjuicios derivados de alteraciones que terceros puedan causar en los sistemas informáticos, documentos electrónicos o ficheros del usuario.<br><br>Si optas por abandonar nuestro sitio web a través de enlaces a sitios web no pertenecientes a nuestra entidad, <strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong> no se hará responsable de las políticas de privacidad de dichos sitios web ni de las cookies que éstos puedan almacenar en el ordenador del usuario.<br><br>Nuestra política con respecto al envío de nuestros correos electrónicos se centra en remitir únicamente comunicaciones que tú hayas solicitado recibir. Si prefiere no recibir estos mensajes por correo electrónico te ofreceremos a través de los mismos la posibilidad de ejercer tu derecho de supresión y renuncia a la recepción de estos mensajes, en conformidad con lo dispuesto en el Título III, artículo 22 de la Ley 34/2002, de Servicios para la Sociedad de la Información y de Comercio Electrónico.</p>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingFour">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="true" aria-controls="collapseFour">
                                        GDPR
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseFour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                                <div class="panel-body">
                                    A wonderful serenity has taken possession of my entire soul, like these sweet mornings of spring which I enjoy with my whole heart. I am alone, and feel the charm of existence in this spot, which was created for the bliss of souls like mine.
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptFiles" runat="server">
    <script type="text/javascript" src="/Admin/Company/Services.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        pageType = "PageAdmin";
        MenuSelectOption("AdminCompany");
    </script>
</asp:Content>