using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using BeenJammin.SharePoint.CascadingLookups;


namespace BeenJammin.SharePoint.WebControls
{
    /// <summary>
    /// EditorControl is used to present and manage the UI for administrators adding our field to a List
    /// </summary>
    public class ParentDropDownListFieldEditControl : UserControl, IFieldEditor
    {

        //couldn't get working, just use the delimited example!!!!
        #region Properties
        protected DropDownList ddlLists;
        protected DropDownList ddlColumnValue;
        protected DropDownList ddlColumnText;
        protected TextBox txtSiteURL;
        protected Button btnLoadLists;
        private string parentSiteUrl = default(string);
        private string parentListName = default(string);
        private string parentListTextField = default(string);
        private string parentListValueField = default(string);

        #endregion

        /// <summary>
        /// Used when editing our field.  Repopulate UI fields with stored values.  Parse delimited value into constituent parts and retrieve 
        /// Site, List and Column values.
        /// </summary>
        /// <param name="field">The field being edited</param>
        public void InitializeWithField(SPField field)
        {
            ParentDropDownListField ParentField = field as ParentDropDownListField;
            if (ParentField != null)
            {
                parentSiteUrl = ParentField.ParentSiteUrl;
                parentListName = ParentField.ParentListName;
                parentListTextField = ParentField.ParentListTextField;
                parentListValueField = ParentField.ParentListValueField;

                EnsureChildControls();

                if (!string.IsNullOrEmpty(parentSiteUrl))
                {
                    txtSiteURL.Text = parentSiteUrl;
                    loadLists();
                    ddlLists.Items.FindByText(parentListName).Selected = true;
                    refreshLookups();
                    ddlColumnText.Items.FindByText(parentListTextField).Selected = true;
                    ddlColumnValue.Items.FindByText(parentListValueField).Selected = true;
                }
            }
        }

        /// <summary>
        /// Event fired when our field values are being saved
        /// </summary>
        /// <param name="field"></param>
        /// <param name="isNew"></param>
        public void OnSaveChange(SPField field, bool isNew)
        {
            parentSiteUrl = this.txtSiteURL.Text;
            parentListName = this.ddlLists.SelectedValue;
            parentListTextField= this.ddlColumnText.SelectedValue;
            parentListValueField = this.ddlColumnValue.SelectedValue;

            ParentDropDownListField ParentField = field as ParentDropDownListField;

            //workaround when the field is new SetCustomProperty doesn't seem to work

            if (isNew)
            {
                ParentField.UpdateMyCustomProperty("ParentSiteUrl", parentSiteUrl);
                ParentField.UpdateMyCustomProperty("ParentListName", parentListName);
                ParentField.UpdateMyCustomProperty("ParentListTextField", parentListTextField);
                ParentField.UpdateMyCustomProperty("ParentListValueField", parentListValueField);
            }
            else
            {
                ParentField.ParentSiteUrl = parentSiteUrl;
                ParentField.ParentListName = parentListName;
                ParentField.ParentListTextField = parentListTextField;
                ParentField.ParentListValueField = parentListValueField;
            }
            //ChildField.Update();
        }

        /// <summary>
        /// Indicate whether our field should be shown as a new section or appended to the end of the preceding section
        /// </summary>
        public bool DisplayAsNewSection
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.btnLoadLists.Click += new EventHandler(btnLoadLists_Click);
            this.ddlLists.AutoPostBack = true;
            this.ddlLists.EnableViewState = true;
            this.ddlLists.SelectedIndexChanged += new EventHandler(ddlLists_SelectedIndexChanged);

            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.parentListName))
                {
                    txtSiteURL.Text = this.parentSiteUrl;
                    loadLists();
                    ddlLists.SelectedValue = this.parentListName;
                    refreshLookups();
                    ddlColumnText.SelectedValue = this.parentListTextField;
                    ddlColumnValue.SelectedValue = this.parentListValueField;
                }
                else
                {
                    //get current site if this is a new field
                    txtSiteURL.Text = SPContext.Current.Site.Url;
                }
            }
        }

        void ddlLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshLookups();
        }

        void btnLoadLists_Click(object sender, EventArgs e)
        {
            loadLists();
        }


        /// <summary>
        /// Handles the click event for the Load Lists button
        /// Retrieves the URL for the site and then populates the ddlLists dropdown list with the names of the available lists from that site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void loadLists()
        {
            SPSite site = null;
            SPWeb web = null;
            try
            {
                EnsureChildControls();
                ddlLists.Items.Clear();
                site = new SPSite(this.txtSiteURL.Text);
                if (site != null)
                {
                    web = site.OpenWeb();
                    foreach (SPList list in web.Lists)
                    {
                        ddlLists.Items.Add(list.Title);
                    }
                }
            }
            catch (Exception ex)
            {
                this.txtSiteURL.Text = ex.Message;
            }
            finally
            {
                site.Dispose();
                web.Dispose();
            }
        }

        /// <summary>
        /// Retrieve and present columns available on selected list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void refreshLookups()
        {
            SPSite site = null;
            SPWeb web = null;
            try
            {
                if (this.ddlLists.SelectedItem != null)
                {
                    EnsureChildControls();
                    ddlColumnText.Items.Clear();
                    ddlColumnValue.Items.Clear();
                    site = new SPSite(txtSiteURL.Text);
                    web = site.OpenWeb();
                    SPList list = web.Lists[this.ddlLists.SelectedItem.Text];

                    foreach (SPField oneField in list.Fields)
                    {
                        ddlColumnText.Items.Add(oneField.Title);
                        ddlColumnValue.Items.Add(oneField.Title);
                    }
                }
            }
            catch (Exception ex)
            {
                this.txtSiteURL.Text = ex.Message;
            }
            finally
            {
                site.Dispose();
                web.Dispose();
            }
        }
    }
}
