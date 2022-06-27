<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CompanyProfile.aspx.cs" Inherits="OpenFrameworkV2.Web.Admin.Company.CompanyProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StylesHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <div class="row" id="PersistentFields">
        <div class="col-lg-12">
            <div class="form-group">
                <label for="Nombre" id="NombreLabel" class="formFieldLabel col-sm-1">Nombre<span class="FieldRequired" style="color: #ff0000;">*</span></label><div class="col-sm-7">
                    <input type="text" id="Title" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="150" onblur="this.value = $.trim(this.value);" spellcheck="false" data-required="true">
                </div>
                <label for="FacturadorId" id="FacturadorIdLabel" class="formFieldLabel col-sm-1">Facturador<span class="FieldRequired" style="color: #ff0000;">*</span></label><div class="col-sm-3" id="DivFacturadorId" title="" data-rel="tooltip">
                    <select class="form-control col-xs-12 col-sm-12 chosen-select formData" id="FacturadorId" data-required="true" style="width: 99%; display: none;">
                        <option value="">Seleccionar</option>
                        <option value="2">Bertamager, S.L.</option>
                        <option value="6">CEXPA Promociones&amp;Merchandising, S.L</option>
                        <option value="1">Inmobiliaria Garrido, S.L.</option>
                        <option value="4">Pepito Pérez</option>
                        <option value="5">RicaCarm, S.L.</option>
                        <option value="7">VICALMA INVEST S.L.</option>
                    </select><div class="chosen-container chosen-container-single" title="" id="FacturadorId_chosen" style="width: 233px;">
                        <a class="chosen-single"></a>
                        <div class="chosen-drop">
                            <div class="chosen-search">
                                <input class="chosen-search-input" type="text" autocomplete="off">
                            </div>
                            <ul class="chosen-results"></ul>
                        </div>
                    </div>
                </div>
                <label for="Direccion" id="DireccionLabel" class="formFieldLabel col-sm-1">Dirección<span class="FieldRequired" style="color: #ff0000;">*</span></label><div class="col-sm-7">
                    <input type="text" id="Direccion" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="150" onblur="this.value = $.trim(this.value);" spellcheck="false" data-required="true">
                </div>
                <label for="Localidad" id="LocalidadLabel" class="formFieldLabel col-sm-1">Localidad<span class="FieldRequired" style="color: #ff0000;">*</span></label><div class="col-sm-3">
                    <input type="text" id="Localidad" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false" data-required="true">
                </div>
            </div>
        </div>
    </div>
    <div class="col-lg-12">

        <div class="hpanel">
            <ul class="nav nav-tabs">
                <li class="active"><a data-toggle="tab" href="#tab-1"><i class="fa fa-laptop"></i>This is tab</a></li>
                <li class=""><a data-toggle="tab" href="#tab-2">This is second tab</a></li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" data-toggle="dropdown"
                        href="#" role="button" aria-haspopup="true" aria-expanded="false">Scripts</a>
                    <div class="dropdown-menu">
                        <a data-toggle="tab"  class="dropdown-item" href="#tab-3">Instancia</a>
                        <a data-toggle="tab"  class="dropdown-item" href="#tab-4">Documentos</a>
                    </div>
                </li>
            </ul>
            <div class="tab-content" style="overflow-x: hidden; overflow-y: auto; max-height: 300px;">
                <div id="tab-1" class="tab-pane active">
                    <div class="hpanel">
                        <div class="panel-body">
                            <form method="get" class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-1 control-label">Normal</label>

                                    <div class="col-sm-5">
                                        <input type="text" class="form-control"></div>
                                    <label class="col-sm-1 control-label">Help text</label>

                                    <div class="col-sm-5">
                                        <input type="text" class="form-control">
                                        <span class="help-block m-b-none">A block of help text that breaks onto a new line and may extend beyond one line.</span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-1 control-label">Password</label>

                                    <div class="col-sm-5">
                                        <input type="password" class="form-control" name="password"></div>
                                    <label class="col-sm-1 control-label">Placeholder</label>

                                    <div class="col-sm-5">
                                        <input type="text" placeholder="placeholder" class="form-control"></div>
                                </div>
                                <div class="hr-line-dashed"></div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Disabled</label>

                                    <div class="col-lg-10">
                                        <input type="text" disabled="" placeholder="Disabled input here..." class="form-control"></div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Static control</label>

                                    <div class="col-lg-10">
                                        <p class="form-control-static">email@example.com</p>
                                    </div>
                                </div>
                                <div class="hr-line-dashed"></div>
                            </form>
                        </div>
                    </div>
                    <div class="hpanel">
            
                        <div class="panel-heading hbuilt">
                            <div class="panel-tools">
                                <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                <a class="closebox"><i class="fa fa-times"></i></a>
                            </div>
                            Panel with alert
                        </div>
            <div class="panel-body" style="padding-top:0;">
                <table id="example1" style="height:470px" class="scroll table table-bordered table-striped" data-page-size="8" data-filter="#filter">
                        <thead>
                        <tr>
                            <th data-toggle="true" class="sort ASC  footable-first-column footable-sortable">Project<span class="footable-sort-indicator"></span></th>
                            <th class=" sort footable-visible footable-sortable">Name<span class="footable-sort-indicator"></span></th>
                            <th class="footable-visible footable-sortable">Phone<span class="footable-sort-indicator"></span></th>
                            <th class="footable-visible footable-last-column footable-sortable">Company<span class="footable-sort-indicator"></span></th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr class="footable-even" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha projectAlpha projectAlpha projectAlpha projectAlpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td class="table-hidden"><span class="pie">6,9</span></td>
                            <td class="table-hidden">40%</td>
                            <td class="table-hidden">Jul 16, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Betha project</td>
                            <td class="footable-visible">John Smith</td>
                            <td class="footable-visible">0800 1111</td>
                            <td class="footable-visible footable-last-column">Erat Volutpat</td>
                            <td class="table-hidden"><span class="pie">3,1</span></td>
                            <td class="table-hidden">75%</td>
                            <td class="table-hidden">Jul 18, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td class="table-hidden"><span class="pie">4,9</span></td>
                            <td class="table-hidden">18%</td>
                            <td class="table-hidden">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In CompanyNec Euismod In CompanyNec Euismod In Company</td>
                            <td class="table-hidden"><span class="pie">6,9</span></td>
                            <td class="table-hidden">40%</td>
                            <td class="table-hidden">Jul 16, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td class="table-hidden"><span class="pie">4,9</span></td>
                            <td class="table-hidden">18%</td>
                            <td class="table-hidden">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td class="table-hidden"><span class="pie">6,9</span></td>
                            <td class="table-hidden">40%</td>
                            <td class="table-hidden">Jul 16, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Betha project</td>
                            <td class="footable-visible">John Smith</td>
                            <td class="footable-visible">0800 1111</td>
                            <td class="footable-visible footable-last-column">Erat Volutpat</td>
                            <td class="table-hidden"><span class="pie">3,1</span></td>
                            <td class="table-hidden">75%</td>
                            <td class="table-hidden">Jul 18, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td class="table-hidden"><span class="pie">4,9</span></td>
                            <td class="table-hidden">18%</td>
                            <td class="table-hidden">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td class="table-hidden"><span class="pie">6,9</span></td>
                            <td class="table-hidden">40%</td>
                            <td class="table-hidden">Jul 16, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td class="table-hidden"><span class="pie">4,9</span></td>
                            <td class="table-hidden">18%</td>
                            <td class="table-hidden">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Betha project</td>
                            <td class="footable-visible">John Smith</td>
                            <td class="footable-visible">0800 1111</td>
                            <td class="footable-visible footable-last-column">Erat Volutpat</td>
                            <td class="table-hidden"><span class="pie">3,1</span></td>
                            <td class="table-hidden">75%</td>
                            <td class="table-hidden">Jul 18, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td class="table-hidden"><span class="pie">4,9</span></td>
                            <td class="table-hidden">18%</td>
                            <td class="table-hidden">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td class="table-hidden"><span class="pie">6,9</span></td>
                            <td class="table-hidden">40%</td>
                            <td class="table-hidden">Jul 16, 2013</td>
                        </tr>
                        </tbody>
                        <tfoot>
                        <tr>
                            <td colspan="5" class="footable-visible">
                                <ul class="pagination pull-right"><li class="footable-page-arrow disabled"><a data-page="first" href="#first">«</a></li><li class="footable-page-arrow disabled"><a data-page="prev" href="#prev">‹</a></li><li class="footable-page active"><a data-page="0" href="#">1</a></li><li class="footable-page"><a data-page="1" href="#">2</a></li><li class="footable-page-arrow"><a data-page="next" href="#next">›</a></li><li class="footable-page-arrow"><a data-page="last" href="#last">»</a></li></ul>
                            </td>
                        </tr>
                        </tfoot>
                    </table>
            </div>
        </div>

                    <div class="hpanel">
                        <div class="panel-heading hbuilt">
                            <div class="panel-tools">
                                <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                <a class="closebox"><i class="fa fa-times"></i></a>
                            </div>
                            Panel with alert
                        </div>
                        <div class="alert alert-success">
                            <i class="fa fa-bolt"></i>Adding action was successful
                        </div>
                        <div class="panel-body" style="display: block;">
                            <p>
                                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum tincidunt est vitae ultrices accumsan. Aliquam ornare lacus adipiscing, posuere lectus et, fringilla augue.Lorem
                    ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum tincidunt est vitae ultrices accumsan.
                            </p>
                        </div>
                    </div>
                </div>
                <div id="tab-2" class="tab-pane">
                    <div class="panel-body">

                        <p>
                            <strong>X-editable</strong> library allows you to create editable elements on your page. It includes both popup and inline modes. Please try out demo below to see how it works.
                        </p>
                        <div class="text-center">
                            <i class="fa fa-angle-down fa-2x"></i>
                        </div>

                        <div>
                            <h5>How to use</h5>
                            <p>Markup elements that should be editable. Usually it is <code>&lt;A&gt;</code> element with additional <code>data-*</code> attributes</p>


                            <table id="user" class="table table-bordered table-striped" style="clear: both">
                                <tbody>
                                    <tr>
                                        <td width="35%">Simple text field</td>
                                        <td width="65%"><a href="#" id="username" data-type="text" data-pk="1" data-title="Enter username" class="editable editable-click">superuser</a></td>
                                    </tr>
                                    <tr>
                                        <td>Empty text field, required</td>
                                        <td><a href="#" id="firstname" data-type="text" data-pk="1" data-placement="right" data-placeholder="Required" data-title="Enter your firstname" class="editable editable-click">Empty</a></td>
                                    </tr>
                                    <tr>
                                        <td>Select, local array, custom display</td>
                                        <td><a href="#" id="sex" data-type="select" data-pk="1" data-value="" data-title="Select sex" class="editable editable-click" style="color: rgb(128, 128, 128);">not selected</a></td>
                                    </tr>

                                    <tr>
                                        <td>Combodate (date)</td>
                                        <td><a href="#" id="dob" data-type="combodate" data-value="1984-05-15" data-format="YYYY-MM-DD" data-viewformat="DD/MM/YYYY" data-template="D / MMM / YYYY" data-pk="1" data-title="Select Date of birth" class="editable editable-click">15/05/1984</a></td>
                                    </tr>
                                    <tr>
                                        <td>Combodate (datetime)</td>
                                        <td><a href="#" id="event" data-type="combodate" data-template="D MMM YYYY  HH:mm" data-format="YYYY-MM-DD HH:mm" data-viewformat="MMM D, YYYY, HH:mm" data-pk="1" data-title="Setup event date and time" class="editable editable-click">Empty</a></td>
                                    </tr>



                                    <tr>
                                        <td>Textarea, buttons below. Submit by <i>ctrl+enter</i></td>
                                        <td><a href="#" id="comments" data-type="textarea" data-pk="1" data-placeholder="Your comments here..." data-title="Enter comments" class="editable editable-pre-wrapped editable-click">awesome user!</a></td>
                                    </tr>



                                    <tr>
                                        <td>Checklist</td>
                                        <td><a href="#" id="fruits" data-type="checklist" data-value="2,3" data-title="Select fruits" class="editable editable-click">peach<br>
                                            apple</a></td>
                                    </tr>

                                </tbody>
                            </table>

                            <p class="m-t-lg">
                                All examples and documentation you can find at: <a href="http://vitalets.github.io/x-editable/" target="_blank">http://vitalets.github.io/x-editable/</a>
                            </p>

                        </div>


                    </div>
                </div>
                <div id="tab-3" class="tab-pane">
                    <div class="panel-body">

                        <p>
                            CodeMirror is a versatile text editor implemented in JavaScript for the browser. It is specialized for editing code, and comes with a number of language modes and addons that implement more advanced editing functionality.
                        </p>

                        <textarea id="code1" class="form-control" style="display: none;">
  // EDITOR CONSTRUCTOR

  // A CodeMirror instance represents an editor. This is the object
  // that user code is usually dealing with.

  function CodeMirror(place, options) {
    if (!(this instanceof CodeMirror)) return new CodeMirror(place, options);

    this.options = options = options ? copyObj(options) : {};
    // Determine effective options based on given values and defaults.
    copyObj(defaults, options, false);
    setGuttersForLineNumbers(options);

    var doc = options.value;
    if (typeof doc == "string") doc = new Doc(doc, options.mode);
    this.doc = doc;

    var display = this.display = new Display(place, doc);
    display.wrapper.CodeMirror = this;
    updateGutters(this);
    themeChanged(this);
    if (options.lineWrapping)
      this.display.wrapper.className += " CodeMirror-wrap";
    if (options.autofocus &amp;&amp; !mobile) focusInput(this);

    this.state = {
      keyMaps: [],  // stores maps added by addKeyMap
      overlays: [], // highlighting overlays, as added by addOverlay
      modeGen: 0,   // bumped when mode/overlay changes, used to invalidate highlighting info
      overwrite: false, focused: false,
      suppressEdits: false, // used to disable editing during key handlers when in readOnly mode
      pasteIncoming: false, cutIncoming: false, // help recognize paste/cut edits in readInput
      draggingText: false,
      highlight: new Delayed() // stores highlight worker timeout
    };

    // Override magic textarea content restore that IE sometimes does
    // on our hidden textarea on reload
    if (ie &amp;&amp; ie_version &lt; 11) setTimeout(bind(resetInput, this, true), 20);

    registerEventHandlers(this);
    ensureGlobalHandlers();

    startOperation(this);
    this.curOp.forceUpdate = true;
    attachDoc(this, doc);

    if ((options.autofocus &amp;&amp; !mobile) || activeElt() == display.input)
      setTimeout(bind(onFocus, this), 20);
    else
      onBlur(this);

    for (var opt in optionHandlers) if (optionHandlers.hasOwnProperty(opt))
      optionHandlers[opt](this, options[opt], Init);
    maybeUpdateLineNumberWidth(this);
    for (var i = 0; i &lt; initHooks.length; ++i) initHooks[i](this);
    endOperation(this);
  }

</textarea><div class="CodeMirror cm-s-default">
    <div style="overflow: hidden; position: relative; width: 3px; height: 0px; top: 5px; left: 34px;">
        <textarea autocorrect="off" autocapitalize="off" spellcheck="false" style="position: absolute; padding: 0px; width: 1000px; height: 1em; outline: none;" tabindex="0"></textarea>
    </div>
    <div class="CodeMirror-hscrollbar" style="left: 29px;">
        <div style="height: 100%; min-height: 1px; width: 0px;"></div>
    </div>
    <div class="CodeMirror-vscrollbar" style="bottom: 0px;">
        <div style="min-width: 1px; height: 0px;"></div>
    </div>
    <div class="CodeMirror-scrollbar-filler"></div>
    <div class="CodeMirror-gutter-filler"></div>
    <div class="CodeMirror-scroll" tabindex="-1">
        <div class="CodeMirror-sizer" style="margin-left: 29px; min-width: 693.188px; min-height: 801px;">
            <div style="position: relative; top: 0px;">
                <div class="CodeMirror-lines">
                    <div style="position: relative; outline: none;">
                        <div class="CodeMirror-measure">
                            <div style="width: 50px; height: 50px; overflow-x: scroll;"></div>
                        </div>
                        <div class="CodeMirror-measure"></div>
                        <div style="position: relative; z-index: 1;"></div>
                        <div class="CodeMirror-cursors">
                            <div class="CodeMirror-cursor" style="left: 4px; top: 0px; height: 13px;">&nbsp;</div>
                        </div>
                        <div class="CodeMirror-code" style="">
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">1</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">2</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">  <span class="cm-comment">// EDITOR CONSTRUCTOR</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">3</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">4</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">  <span class="cm-comment">// A CodeMirror instance represents an editor. This is the object</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">5</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">  <span class="cm-comment">// that user code is usually dealing with.</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">6</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">7</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">  <span class="cm-keyword">function</span> <span class="cm-variable">CodeMirror</span>(<span class="cm-def">place</span>, <span class="cm-def">options</span>) {</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">8</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">if</span> (<span class="cm-operator">!</span>(<span class="cm-keyword">this</span> <span class="cm-keyword">instanceof</span> <span class="cm-variable">CodeMirror</span>)) <span class="cm-keyword">return</span> <span class="cm-keyword">new</span> <span class="cm-variable">CodeMirror</span>(<span class="cm-variable-2">place</span>, <span class="cm-variable-2">options</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">9</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">10</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">this</span>.<span class="cm-property">options</span> <span class="cm-operator">=</span> <span class="cm-variable-2">options</span> <span class="cm-operator">=</span> <span class="cm-variable-2">options</span> <span class="cm-operator">?</span> <span class="cm-variable">copyObj</span>(<span class="cm-variable-2">options</span>) : {};</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">11</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-comment">// Determine effective options based on given values and defaults.</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">12</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">copyObj</span>(<span class="cm-variable">defaults</span>, <span class="cm-variable-2">options</span>, <span class="cm-atom">false</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">13</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">setGuttersForLineNumbers</span>(<span class="cm-variable-2">options</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">14</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">15</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">var</span> <span class="cm-def">doc</span> <span class="cm-operator">=</span> <span class="cm-variable-2">options</span>.<span class="cm-property">value</span>;</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">16</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">if</span> (<span class="cm-keyword">typeof</span> <span class="cm-variable-2">doc</span> <span class="cm-operator">==</span> <span class="cm-string">"string"</span>) <span class="cm-variable-2">doc</span> <span class="cm-operator">=</span> <span class="cm-keyword">new</span> <span class="cm-variable">Doc</span>(<span class="cm-variable-2">doc</span>, <span class="cm-variable-2">options</span>.<span class="cm-property">mode</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">17</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">this</span>.<span class="cm-property">doc</span> <span class="cm-operator">=</span> <span class="cm-variable-2">doc</span>;</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">18</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">19</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">var</span> <span class="cm-def">display</span> <span class="cm-operator">=</span> <span class="cm-keyword">this</span>.<span class="cm-property">display</span> <span class="cm-operator">=</span> <span class="cm-keyword">new</span> <span class="cm-variable">Display</span>(<span class="cm-variable-2">place</span>, <span class="cm-variable-2">doc</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">20</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable-2">display</span>.<span class="cm-property">wrapper</span>.<span class="cm-property">CodeMirror</span> <span class="cm-operator">=</span> <span class="cm-keyword">this</span>;</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">21</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">updateGutters</span>(<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">22</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">themeChanged</span>(<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">23</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">if</span> (<span class="cm-variable-2">options</span>.<span class="cm-property">lineWrapping</span>)</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">24</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-keyword">this</span>.<span class="cm-property">display</span>.<span class="cm-property">wrapper</span>.<span class="cm-property">className</span> <span class="cm-operator">+=</span> <span class="cm-string">" CodeMirror-wrap"</span>;</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">25</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">if</span> (<span class="cm-variable-2">options</span>.<span class="cm-property">autofocus</span> <span class="cm-operator">&amp;&amp;</span> <span class="cm-operator">!</span><span class="cm-variable">mobile</span>) <span class="cm-variable">focusInput</span>(<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">26</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">27</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">this</span>.<span class="cm-property">state</span> <span class="cm-operator">=</span> {</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">28</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-property">keyMaps</span>: [],  <span class="cm-comment">// stores maps added by addKeyMap</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">29</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-property">overlays</span>: [], <span class="cm-comment">// highlighting overlays, as added by addOverlay</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">30</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-property">modeGen</span>: <span class="cm-number">0</span>,   <span class="cm-comment">// bumped when mode/overlay changes, used to invalidate highlighting info</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">31</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-property">overwrite</span>: <span class="cm-atom">false</span>, <span class="cm-property">focused</span>: <span class="cm-atom">false</span>,</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">32</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-property">suppressEdits</span>: <span class="cm-atom">false</span>, <span class="cm-comment">// used to disable editing during key handlers when in readOnly mode</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">33</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-property">pasteIncoming</span>: <span class="cm-atom">false</span>, <span class="cm-property">cutIncoming</span>: <span class="cm-atom">false</span>, <span class="cm-comment">// help recognize paste/cut edits in readInput</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">34</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-property">draggingText</span>: <span class="cm-atom">false</span>,</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">35</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-property">highlight</span>: <span class="cm-keyword">new</span> <span class="cm-variable">Delayed</span>() <span class="cm-comment">// stores highlight worker timeout</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">36</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    };</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">37</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">38</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-comment">// Override magic textarea content restore that IE sometimes does</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">39</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-comment">// on our hidden textarea on reload</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">40</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">if</span> (<span class="cm-variable">ie</span> <span class="cm-operator">&amp;&amp;</span> <span class="cm-variable">ie_version</span> <span class="cm-operator">&lt;</span> <span class="cm-number">11</span>) <span class="cm-variable">setTimeout</span>(<span class="cm-variable">bind</span>(<span class="cm-variable">resetInput</span>, <span class="cm-keyword">this</span>, <span class="cm-atom">true</span>), <span class="cm-number">20</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">41</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">42</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">registerEventHandlers</span>(<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">43</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">ensureGlobalHandlers</span>();</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">44</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">45</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">startOperation</span>(<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">46</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">this</span>.<span class="cm-property">curOp</span>.<span class="cm-property">forceUpdate</span> <span class="cm-operator">=</span> <span class="cm-atom">true</span>;</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">47</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">attachDoc</span>(<span class="cm-keyword">this</span>, <span class="cm-variable-2">doc</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">48</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">49</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">if</span> ((<span class="cm-variable-2">options</span>.<span class="cm-property">autofocus</span> <span class="cm-operator">&amp;&amp;</span> <span class="cm-operator">!</span><span class="cm-variable">mobile</span>) <span class="cm-operator">||</span> <span class="cm-variable">activeElt</span>() <span class="cm-operator">==</span> <span class="cm-variable-2">display</span>.<span class="cm-property">input</span>)</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">50</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-variable">setTimeout</span>(<span class="cm-variable">bind</span>(<span class="cm-variable">onFocus</span>, <span class="cm-keyword">this</span>), <span class="cm-number">20</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">51</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">else</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">52</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-variable">onBlur</span>(<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">53</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">54</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">for</span> (<span class="cm-keyword">var</span> <span class="cm-def">opt</span> <span class="cm-keyword">in</span> <span class="cm-variable">optionHandlers</span>) <span class="cm-keyword">if</span> (<span class="cm-variable">optionHandlers</span>.<span class="cm-property">hasOwnProperty</span>(<span class="cm-variable-2">opt</span>))</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">55</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">      <span class="cm-variable">optionHandlers</span>[<span class="cm-variable-2">opt</span>](<span class="cm-keyword">this</span>, <span class="cm-variable-2">options</span>[<span class="cm-variable-2">opt</span>], <span class="cm-variable">Init</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">56</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">maybeUpdateLineNumberWidth</span>(<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">57</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-keyword">for</span> (<span class="cm-keyword">var</span> <span class="cm-def">i</span> <span class="cm-operator">=</span> <span class="cm-number">0</span>; <span class="cm-variable-2">i</span> <span class="cm-operator">&lt;</span> <span class="cm-variable">initHooks</span>.<span class="cm-property">length</span>; <span class="cm-operator">++</span><span class="cm-variable-2">i</span>) <span class="cm-variable">initHooks</span>[<span class="cm-variable-2">i</span>](<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">58</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">    <span class="cm-variable">endOperation</span>(<span class="cm-keyword">this</span>);</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">59</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;">  }</span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">60</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                            <div style="position: relative;">
                                <div class="CodeMirror-gutter-wrapper" style="position: absolute; left: -29px;">
                                    <div class="CodeMirror-linenumber CodeMirror-gutter-elt" style="left: 0px; width: 20px;">61</div>
                                </div>
                                <pre class=""><span style="padding-right: 0.1px;"><span>​</span></span></pre>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="position: absolute; height: 30px; width: 1px; top: 801px;"></div>
        <div class="CodeMirror-gutters" style="height: 801px;">
            <div class="CodeMirror-gutter CodeMirror-linenumbers" style="width: 28px;"></div>
        </div>
    </div>
</div>

                    </div>
                </div>

                <div id="tab-4" class="tab-pane">
                    <div class="panel-body">

                        <p>
                            Bootstrap-datepicker provides a flexible datepicker widget in the Twitter bootstrap style.
                        http://bootstrap-datepicker.readthedocs.org/en/latest/index.html
                        </p>

                        <div class="row">

                            <div class="col-lg-3">
                                <strong>Inline version
                                </strong>

                                <div id="datepicker" data-date="12/03/2012">
                                    <div class="datepicker datepicker-inline">
                                        <div class="datepicker-days" style="display: block;">
                                            <table class=" table-condensed">
                                                <thead>
                                                    <tr>
                                                        <th class="prev" style="visibility: visible;">«</th>
                                                        <th colspan="5" class="datepicker-switch">December 2012</th>
                                                        <th class="next" style="visibility: visible;">»</th>
                                                    </tr>
                                                    <tr>
                                                        <th class="dow">Su</th>
                                                        <th class="dow">Mo</th>
                                                        <th class="dow">Tu</th>
                                                        <th class="dow">We</th>
                                                        <th class="dow">Th</th>
                                                        <th class="dow">Fr</th>
                                                        <th class="dow">Sa</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td class="old day">25</td>
                                                        <td class="old day">26</td>
                                                        <td class="old day">27</td>
                                                        <td class="old day">28</td>
                                                        <td class="old day">29</td>
                                                        <td class="old day">30</td>
                                                        <td class="day">1</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="day">2</td>
                                                        <td class="active day">3</td>
                                                        <td class="day">4</td>
                                                        <td class="day">5</td>
                                                        <td class="day">6</td>
                                                        <td class="day">7</td>
                                                        <td class="day">8</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="day">9</td>
                                                        <td class="day">10</td>
                                                        <td class="day">11</td>
                                                        <td class="day">12</td>
                                                        <td class="day">13</td>
                                                        <td class="day">14</td>
                                                        <td class="day">15</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="day">16</td>
                                                        <td class="day">17</td>
                                                        <td class="day">18</td>
                                                        <td class="day">19</td>
                                                        <td class="day">20</td>
                                                        <td class="day">21</td>
                                                        <td class="day">22</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="day">23</td>
                                                        <td class="day">24</td>
                                                        <td class="day">25</td>
                                                        <td class="day">26</td>
                                                        <td class="day">27</td>
                                                        <td class="day">28</td>
                                                        <td class="day">29</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="day">30</td>
                                                        <td class="day">31</td>
                                                        <td class="new day">1</td>
                                                        <td class="new day">2</td>
                                                        <td class="new day">3</td>
                                                        <td class="new day">4</td>
                                                        <td class="new day">5</td>
                                                    </tr>
                                                </tbody>
                                                <tfoot>
                                                    <tr>
                                                        <th colspan="7" class="today" style="display: none;">Today</th>
                                                    </tr>
                                                    <tr>
                                                        <th colspan="7" class="clear" style="display: none;">Clear</th>
                                                    </tr>
                                                </tfoot>
                                            </table>
                                        </div>
                                        <div class="datepicker-months" style="display: none;">
                                            <table class="table-condensed">
                                                <thead>
                                                    <tr>
                                                        <th class="prev" style="visibility: visible;">«</th>
                                                        <th colspan="5" class="datepicker-switch">2012</th>
                                                        <th class="next" style="visibility: visible;">»</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td colspan="7"><span class="month">Jan</span><span class="month">Feb</span><span class="month">Mar</span><span class="month">Apr</span><span class="month">May</span><span class="month">Jun</span><span class="month">Jul</span><span class="month">Aug</span><span class="month">Sep</span><span class="month">Oct</span><span class="month">Nov</span><span class="month active">Dec</span></td>
                                                    </tr>
                                                </tbody>
                                                <tfoot>
                                                    <tr>
                                                        <th colspan="7" class="today" style="display: none;">Today</th>
                                                    </tr>
                                                    <tr>
                                                        <th colspan="7" class="clear" style="display: none;">Clear</th>
                                                    </tr>
                                                </tfoot>
                                            </table>
                                        </div>
                                        <div class="datepicker-years" style="display: none;">
                                            <table class="table-condensed">
                                                <thead>
                                                    <tr>
                                                        <th class="prev" style="visibility: visible;">«</th>
                                                        <th colspan="5" class="datepicker-switch">2010-2019</th>
                                                        <th class="next" style="visibility: visible;">»</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td colspan="7"><span class="year old">2009</span><span class="year">2010</span><span class="year">2011</span><span class="year active">2012</span><span class="year">2013</span><span class="year">2014</span><span class="year">2015</span><span class="year">2016</span><span class="year">2017</span><span class="year">2018</span><span class="year">2019</span><span class="year new">2020</span></td>
                                                    </tr>
                                                </tbody>
                                                <tfoot>
                                                    <tr>
                                                        <th colspan="7" class="today" style="display: none;">Today</th>
                                                    </tr>
                                                    <tr>
                                                        <th colspan="7" class="clear" style="display: none;">Clear</th>
                                                    </tr>
                                                </tfoot>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <input type="hidden" id="my_hidden_input">
                            </div>
                            <div class="col-lg-9">

                                <div class="row">

                                    <div class="col-md-6">
                                        <h5>Input
                                        </h5>

                                        <p>
                                            The simplest case: focusing the input (clicking or tabbing into it) will show the picker.
                                        </p>
                                        <input id="datapicker2" type="text" value="02-16-2012" class="form-control">

                                        <h5>Component</h5>

                                        <p>
                                            Adding the date class to an input-group bootstrap component will allow the input-group-addon elements to trigger the picker.
                                        </p>

                                        <div class="input-group date">
                                            <input type="text" class="form-control"><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <h5>Range
                                        </h5>

                                        <p>
                                            Using the input-daterange construct with multiple child inputs will instantiate one picker per input and link them together to allow selecting ranges.
                                        </p>

                                        <div class="input-daterange input-group" id="datepicker">
                                            <input type="text" class="input-sm form-control" name="start">
                                            <span class="input-group-addon">to</span>
                                            <input type="text" class="input-sm form-control" name="end">
                                        </div>

                                        <h5>Options</h5>

                                        <p>
                                            There are many avalible options to datapicker, check: <a href="https://bootstrap-datepicker.readthedocs.org/en/latest/options.html" target="_blank">https://bootstrap-datepicker.readthedocs.org/en/latest/options.html</a>
                                        </p>
                                    </div>

                                </div>


                            </div>

                        </div>


                    </div>
                </div>
            </div>


        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    pageType = "admin";
</asp:Content>