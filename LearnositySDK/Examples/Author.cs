using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Examples
{
    public class Author
    {
        public static string Simple(string mode)
        {
            // Author API contains 2 modes
            // item_edit mode and item_list mode
            // the below example generate initOptions for item_edit mode
            // more information about item_list mode can be found in initializeItemList() function
            Init init = (mode == "item_edit") ? initializeItemEdit() : initializeItemList();
            return init.generate();
        }

        private static Init initializeItemEdit() {
            string service = "author";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            string secret = Credentials.ConsumerSecret;

            JsonObject request = new JsonObject();
            request.set("mode", "item_edit");
            request.set("reference", Uuid.generate());

            JsonObject config = new JsonObject();
            JsonObject config_item_edit = new JsonObject();
            JsonObject config_item_edit_widget = new JsonObject();
            config_item_edit_widget.set("delete", true);
            config_item_edit_widget.set("edit", true);
            config_item_edit.set("widget", config_item_edit_widget);
            JsonObject config_item_edit_item = new JsonObject();
            JsonObject config_item_edit_item_tags = new JsonObject();
            JsonObject config_item_edit_item_tags_includesTagOnEdit = new JsonObject(true);
            JsonObject config_item_edit_item_tags_includesTagOnEdit_tag = new JsonObject();
            config_item_edit_item_tags_includesTagOnEdit_tag.set("type", "course");
            config_item_edit_item_tags_includesTagOnEdit_tag.set("name", "commoncore");
            config_item_edit_item_tags_includesTagOnEdit.set(config_item_edit_item_tags_includesTagOnEdit_tag);
            config_item_edit_item_tags.set("include_tags_on_edit", config_item_edit_item_tags_includesTagOnEdit);
            config_item_edit_item.set("tags", config_item_edit_item_tags);
            config_item_edit.set("item", config_item_edit_item);
            config.set("item_edit", config_item_edit);

            JsonObject config_questionEditorInitOptions = new JsonObject();
            JsonObject config_questionEditorInitOptions_ui = new JsonObject();
            config_questionEditorInitOptions_ui.set("question_tiles", false);
            config_questionEditorInitOptions_ui.set("documentation_link", false);
            config_questionEditorInitOptions_ui.set("change_button", true);
            config_questionEditorInitOptions_ui.set("source_button", false);
            config_questionEditorInitOptions_ui.set("fixed_preview", true);
            config_questionEditorInitOptions_ui.set("advanced_group", false);
            config_questionEditorInitOptions_ui.set("search_field", false);
            config_questionEditorInitOptions.set("ui", config_questionEditorInitOptions_ui);
            config.set("question_editor_init_options", config_questionEditorInitOptions);

            request.set("config", config);

            JsonObject user = new JsonObject();
            user.set("id", "brianmoser");
            user.set("firstname", "Test");
            user.set("lastname", "Test");
            user.set("email", "test@test.com");
            request.set("user", user);
            

            return new Init(service, security, secret, request);
        }

        private static Init initializeItemList() {
            string service = "author";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            string secret = Credentials.ConsumerSecret;

            JsonObject request = new JsonObject();
            request.set("mode", "item_list");

            JsonObject config = new JsonObject();
            JsonObject config_item_list = new JsonObject();
            JsonObject config_item_list_toolbar = new JsonObject();
            config_item_list_toolbar.set("add", true);
            config_item_list.set("toolbar", config_item_list_toolbar);
            config.set("item_list", config_item_list);
            request.set("config", config);

            JsonObject tags = new JsonObject(true);
            JsonObject tag = new JsonObject();
            tag.set("type", "course");
            tag.set("name", "commoncore");
            tags.set(tag);
            request.set("tags", tags);

            JsonObject user = new JsonObject();
            user.set("id", "brianmoser");
            user.set("firstname", "Test");
            user.set("lastname", "Test");
            user.set("email", "test@test.com");
            request.set("user", user);

            return new Init(service, security, secret, request);
        }
    }
}
