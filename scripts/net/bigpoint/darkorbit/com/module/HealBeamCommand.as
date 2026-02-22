package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class HealBeamCommand implements IModule
   {
      private static var _instance:HealBeamCommand;
      
      public var sourceId:String = "";
      
      public var sourcePositionX:int = 0;
      
      public var sourcePositionY:int = 0;
      
      public var targetId:int = 0;
      
      public function HealBeamCommand(param1:String = "", param2:int = 0, param3:int = 0, param4:int = 0)
      {
         super();
         this.sourceId = param1;
         this.sourcePositionX = param2;
         this.sourcePositionY = param3;
         this.targetId = param4;
      }
      
      public static function get instance() : HealBeamCommand
      {
         return _instance || (_instance = new HealBeamCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 7779;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 14;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.sourceId = param1.readUTF();
         this.sourcePositionX = param1.readInt();
         this.sourcePositionX = this.sourcePositionX >>> 15 | this.sourcePositionX << 17;
         this.sourcePositionY = param1.readInt();
         this.sourcePositionY = this.sourcePositionY << 5 | this.sourcePositionY >>> 27;
         this.targetId = param1.readInt();
         this.targetId = this.targetId << 15 | this.targetId >>> 17;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(7779);
         param1.writeUTF(this.sourceId);
         param1.writeInt(this.sourcePositionX << 15 | this.sourcePositionX >>> 17);
         param1.writeInt(this.sourcePositionY >>> 5 | this.sourcePositionY << 27);
         param1.writeInt(this.targetId >>> 15 | this.targetId << 17);
      }
   }
}

