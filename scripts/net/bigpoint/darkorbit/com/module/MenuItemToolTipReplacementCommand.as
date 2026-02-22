package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MenuItemToolTipReplacementCommand implements IModule
   {
      private static var _instance:MenuItemToolTipReplacementCommand;
      
      public var key:String = "";
      
      public var value:String = "";
      
      public function MenuItemToolTipReplacementCommand(param1:String = "", param2:String = "")
      {
         super();
         this.key = param1;
         this.value = param2;
      }
      
      public static function get instance() : MenuItemToolTipReplacementCommand
      {
         return _instance || (_instance = new MenuItemToolTipReplacementCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 24881;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.key = param1.readUTF();
         this.value = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(24881);
         param1.writeUTF(this.key);
         param1.writeUTF(this.value);
      }
   }
}

