namespace DarksideApi.DarkOrbit.Commands.Shop
{
    public class ShopLabelPrompt : Command
    {
        public new const short Id = 29682;
        public string category = string.Empty;
        public ShopLabelPrompt.PromptType promptType;

        public ShopLabelPrompt(string category)
        {
            this.category = category;
        }

        public override void Read(ByteArray reader)
        {
            this.promptType = (PromptType)reader.ReadShort();
            this.category = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum PromptType
        {
            PROMPT_NONE,
            PROMPT_DISCOUNTS,
            PROMPT_ACTIVITY,
            PROMPT_DISCOUNTS_AND_ACTIVITY
        }

        public override string ToString()
        {
            return $"ShopLabelPrompt{{category='{this.category}', promptType={this.promptType}}}\n";
        }
    }
}
