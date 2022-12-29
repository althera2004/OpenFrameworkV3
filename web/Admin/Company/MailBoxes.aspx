<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MailBoxes.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Company.MailBoxes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    
    <div class="row">
        <div class="col-lg-6">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    <%= this.Translate("Core_MailBox_FormTitle1") %>
                </div>
                <div class="panel-body">
                    <p><%=this.Translate("Core_MailBox_Explanation1") %></p>
                    <form method="get" class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_Email") %></label>
                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="MainMailMadress" value="<%=this.Main.MailAddress %>" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_SenderName") %></label>

                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="MainSenderName" value="<%=this.Main.SenderName %>" />
                                <span class="help-block m-b-none"><%=this.Translate("Core_MailBox_Tip1") %></span>
                            </div>
                        </div>
                        <div class="panel-body">
                            <%=this.Translate("Core_MailBox_Tip3") %>
                            <div class="form-group">
                                <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_User") %></label>
                                <div class="col-sm-10">
                                    <input type="text" class="form-control" id="MainMailUser" value="<%=this.Main.MailUser %>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_Password") %></label>

                                <div class="col-sm-10">
                                    <input type="password" class="form-control" id="MainMailPassword" value="<%=this.Main.MailPassword%>" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_Server") %></label>

                                <div class="col-sm-10">
                                    <input type="text" class="form-control" id="MainServer" value="<%=this.Main.Server%>" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-lg-2 control-label"><%=this.Translate("Core_MailBox_Label_Port") %></label>

                                <div class="col-lg-10">
                                    <input type="number" class="form-control" id="MainSendPort" value="<%=this.Main.SendPort %>" />
                                    <span class="help-block m-b-none"><%= this.Translate("Core_MailBox_Tip2") %></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 control-label">SSL/TLS</label>
                                <div class="col-lg-10" style="text-align:left;padding-top:7px;">
                                    <input type="checkbox" id="MainSSL" <%=this.Main.SSL ? " checked=\"checked\"" : string.Empty %>" />
                                </div>
                            </div>
                        </div>


                    </form>
                </div>

                <div class="panel-footer">
                    <label><%=this.Translate("Common_Footer_Label_Actions") %>:</label>&nbsp;
                    <button type="button" id="MainBtnSave" class="btn btn-xs btn-info" onclick="MAILBOXES_SaveMain();"><%=this.Translate("Common_Save") %></button>
                    <button type="button" id="MainBtnSendTest" class="btn btn-xs btn-info"><%=this.Translate("Core_MailBox_Button_SendTest") %></button>
                    <button type="button" id="MainBtnCheck" class="btn btn-danger btn-xs" onclick="MAILBOXES_CheckBlackListMain();"><%=this.Translate("Core_MailBox_Button_BlackList") %></button>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    <%= this.Translate("Core_MailBox_FormTitle2") %>
                </div>
                <div class="panel-body">
                    <p><%=this.Translate("Core_MailBox_Explanation2") %></p>
                    <form method="get" class="form-horizontal">
                        <div class="form-group">
                            <label class="col-xs-12">
                                <input type="checkbox" style="margin:5px 0 0 0!important;" id="ChkSameAddress" <%=this.ThirdParty.Id < 1 ? " checked=\"checked\"" : string.Empty  %> />
                                Usar la misma dirección de correo que para envíos a usuarios.</label>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_Email") %></label>

                            <div class="col-sm-10">
                                <input type="text" id="Third_MailAddress" class="form-control" value="<%=this.ThirdParty.MailAddress %>" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_SenderName") %></label>

                            <div class="col-sm-10">
                                <input type="text" id="Third_SenderName" class="form-control" value="<%=this.ThirdParty.SenderName %>" />
                                <span class="help-block m-b-none"><%=this.Translate("Core_MailBox_Tip1") %></span>
                            </div>
                        </div>
                        <div class="panel-body">
                            <%=this.Translate("Core_MailBox_Tip3") %>
                            <div class="form-group">
                                <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_User") %></label>

                                <div class="col-sm-10">
                                    <input type="password" id="Third_MailUser" class="form-control" name="password" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_Password") %></label>

                                <div class="col-sm-10">
                                    <input type="password" id="Third_MailPassword" class="form-control" name="password" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label"><%=this.Translate("Core_MailBox_Label_Server") %></label>

                                <div class="col-sm-10">
                                    <input type="text" id="Third_Server" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 control-label"><%=this.Translate("Core_MailBox_Label_Port") %></label>

                                <div class="col-lg-10">
                                    <input type="number" id="Third_SendPort" class="form-control" />
                                    <span class="help-block m-b-none"><%= this.Translate("Core_MailBox_Tip2") %></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 control-label">SSL/TLS</label>
                                <div class="col-lg-10" style="text-align:left;padding-top:7px;">
                                    <input type="checkbox" id="Third_SSL" <%=this.ThirdParty.SSL ? " checked=\"checked\"" : string.Empty %>" />
                                </div>
                            </div>
                        </div>


                    </form>
                </div>

                <div class="panel-footer">
                    <label><%=this.Translate("Common_Footer_Label_Actions") %>:</label>&nbsp;
                    <button type="button" id="ThirdBtnSave" class="btn btn-xs btn-info"><%=this.Translate("Common_Save") %></button>
                    <button type="button" id="ThirdBtnSendTest" class="btn btn-xs btn-info"><%=this.Translate("Core_MailBox_Button_SendTest") %></button>
                    <button type="button" id="ThirdBtnCheck" class="btn btn-danger btn-xs"><%=this.Translate("Core_MailBox_Button_BlackList") %></button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        pageType = "admin";
        var mainAddress = <%=this.Main.Json %>;
        var thirdAddress = <%=this.ThirdParty.Json %>;
    </script>
</asp:Content>