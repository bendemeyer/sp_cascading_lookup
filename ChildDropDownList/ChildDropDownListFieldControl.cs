using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

using System.Security.Principal;
using System.Security.Permissions;

using BeenJammin.SharePoint.CascadingLookups;
using Microsoft.SharePoint;
using System.Data;

namespace BeenJammin.SharePoint.WebControls
{
    public class ChildDropDownListFieldControl : BaseFieldControl
    {
        protected DropDownList ChildDropDownList;

        protected string childSiteUrl;
        protected string childListName;
        protected string childListTextField;
        protected string childListValueField;
        protected string childJoinField;
      
        public void SetDataSource(string parentSelectedValue)
        {
            //impersonate SharePoint App Pool Account by passing a null pointer as the id
            WindowsIdentity CurrentIdentity = WindowsIdentity.GetCurrent();
            WindowsImpersonationContext ImpersonationContext = WindowsIdentity.Impersonate(IntPtr.Zero);
            WindowsIdentity.GetCurrent().Impersonate();

            this.ChildDropDownList.Items.Clear();

            childSiteUrl = Field.GetCustomProperty("ChildSiteUrl").ToString();
            childListName = Field.GetCustomProperty("ChildListName").ToString();
            childListTextField = Field.GetCustomProperty("ChildListTextField").ToString();
            childListValueField = Field.GetCustomProperty("ChildListValueField").ToString();
            childJoinField = Field.GetCustomProperty("ChildJoinField").ToString();

            SPSite site = new SPSite(childSiteUrl);
            SPList list = site.OpenWeb().Lists[childListName];

            string caml = @"<Where>
                                <Eq>
                                    <FieldRef Name='{0}'/><Value Type='Text'>{1}</Value>
                                </Eq></Where>";

            SPQuery query = new SPQuery();
            query.Query = string.Format(caml, childJoinField, parentSelectedValue);

            SPListItemCollection results = list.GetItems(query);

            DataTable dt = results.GetDataTable();
            if (dt != null)
            {
                DataView dv = new DataView(dt);
                //if the title field has been renamed it doesn't seem to pick up the changed name 
                //so need to get the internal name for the field
                string childListTextFieldInternal = childListTextField;
                string childListValueFieldInternal = childListValueField;

                if (!String.Equals(childListTextFieldInternal, childListValueFieldInternal))
                {
                    String[] columnCollection = { childListTextFieldInternal, childListValueFieldInternal };
                    this.ChildDropDownList.DataSource = dv.ToTable(true, columnCollection);
                }
                else
                {
                    this.ChildDropDownList.DataSource = dv.ToTable(true, childListTextFieldInternal);
                }

                this.ChildDropDownList.DataTextField = childListTextFieldInternal;
                this.ChildDropDownList.DataValueField = childListValueFieldInternal;
                this.ChildDropDownList.DataBind();

                if (!this.Page.IsPostBack)
                {
                    if (this.ListItem[Field.Id] != null)
                    {
                        ChildDropDownList.SelectedValue = this.ListItem[Field.Id].ToString();
                    }
                }
            }

            //if the title field has been renamed it doesn't seem to pick up the changed name 
            //so need to get the internal name for the field
            /*string childListTextFieldInternal = list.Fields[childListTextField].InternalName;
            string childListValueFieldInternal = list.Fields[childListValueField].InternalName;

            this.ChildDropDownList.DataSource = results.GetDataTable();

            this.ChildDropDownList.DataTextField = childListTextFieldInternal;
            this.ChildDropDownList.DataValueField = childListValueFieldInternal;
            this.ChildDropDownList.DataBind();*/

            ImpersonationContext = WindowsIdentity.Impersonate(CurrentIdentity.Token);
            WindowsIdentity.GetCurrent().Impersonate();

        }


        protected override string DefaultTemplateName
        {
            get
            {
                return "ChildDropDownListFieldControl";
            }
        }

        public override object Value
        {
            get
            {
                EnsureChildControls();
                return ChildDropDownList.SelectedValue;
            }

            set
            {
                EnsureChildControls();
                ChildDropDownList.SelectedValue = (string)this.ItemFieldValue;
            }
        }


        public override void Focus()
        {
            EnsureChildControls();
            ChildDropDownList.Focus();
        }

        protected override void CreateChildControls()
        {
            if (Field == null) return;
            base.CreateChildControls();

            if (ControlMode == Microsoft.SharePoint.WebControls.SPControlMode.Display)
                return;

          
            ChildDropDownList = (DropDownList)TemplateContainer.FindControl("ChildDropDownList");

            if (ChildDropDownList == null)
                throw new ArgumentException("ChildDropDownList is null. Problem with ChildDropDownListFieldControl.ascx file.");

            ChildDropDownList.TabIndex = TabIndex;
            ChildDropDownList.CssClass = CssClass;
            ChildDropDownList.ToolTip = Field.Title + " Parent";
        }
    }
}