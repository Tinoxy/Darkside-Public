package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ItemCommand implements IModule
   {
      private static var _instance:ItemCommand;
      
      public var visible:Boolean = false;
      
      public var toolTip:MessageKeyListWildcardCommand;
      
      public var itemId:String = "";
      
      public function ItemCommand(param1:String = "", param2:Boolean = false, param3:MessageKeyListWildcardCommand = null)
      {
         super();
         this.visible = param2;
         if(param3 == null)
         {
            this.toolTip = new MessageKeyListWildcardCommand();
         }
         else
         {
            this.toolTip = param3;
         }
         this.itemId = param1;
      }
      
      public static function get instance() : ItemCommand
      {
         return _instance || (_instance = new ItemCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 2210;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 3;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.visible = param1.readBoolean();
         this.toolTip = MessageKeyListWildcardCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.toolTip)
         {
            this.toolTip.read(param1);
         }
         this.itemId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(2210);
         param1.writeBoolean(this.visible);
         if(null != this.toolTip)
         {
            this.toolTip.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeUTF(this.itemId);
      }
   }
}

