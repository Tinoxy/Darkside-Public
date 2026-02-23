package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EternalGateSelectionCommand implements IModule
   {
      private static var _instance:EternalGateSelectionCommand;
      
      public var selectedOption:int = 0;
      
      public function EternalGateSelectionCommand(param1:int = 0)
      {
         super();
         this.selectedOption = param1;
      }
      
      public static function get instance() : EternalGateSelectionCommand
      {
         return _instance || (_instance = new EternalGateSelectionCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 1947;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.selectedOption = param1.readInt();
         this.selectedOption = this.selectedOption >>> 12 | this.selectedOption << 20;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(1947);
         param1.writeInt(this.selectedOption << 12 | this.selectedOption >>> 20);
      }
   }
}

