using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Examples
{
    public class Author
    {
        public static string Simple()
        {
            string service = "author";

            JsonObject security = new JsonObject();
            security.set("consumer_key", "yis0TYCu7U9V4o7M");
            security.set("domain", "localhost");

            string secret = "74c5fd430cf1242a527f6223aebd42d30464be22";

            JsonObject request = new JsonObject();
            JsonObject components = new JsonObject(true);
            JsonObject component = new JsonObject();
            JsonObject questionEditorOptions = new JsonObject(true);
            JsonObject ui = new JsonObject();

            ui.set("public_methods", new JsonObject(true));
            ui.set("question_tiles", false);
            ui.set("documentation_link", false);
            ui.set("change_button", true);
            ui.set("source_button", false);
            ui.set("fixed_preview", true);
            ui.set("advanced_group", false);
            ui.set("search_field", false);

            component.set("id", "learnosity_author");
            component.set("type", "itemeditor");
            component.set("reference", Uuid.generate());
            component.set("template", "single-question");

            questionEditorOptions.set("ui", ui);
            component.set("question-editor-options", questionEditorOptions);
            components.set(component);
            request.set("components", components);

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }
    }
}
