package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class QuestLocalizationModule implements IModule
   {
      private static var _instance:QuestLocalizationModule;
      
      public var textKeyFoundation:String = "";
      
      public var questId:int = 0;
      
      public function QuestLocalizationModule(param1:int = 0, param2:String = "")
      {
         super();
         this.textKeyFoundation = param2;
         this.questId = param1;
      }
      
      public static function get instance() : QuestLocalizationModule
      {
         return _instance || (_instance = new QuestLocalizationModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -23954;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.textKeyFoundation = param1.readUTF();
         this.questId = param1.readInt();
         this.questId = this.questId >>> 10 | this.questId << 22;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-23954);
         param1.writeUTF(this.textKeyFoundation);
         param1.writeInt(this.questId << 10 | this.questId >>> 22);
      }
   }
}

