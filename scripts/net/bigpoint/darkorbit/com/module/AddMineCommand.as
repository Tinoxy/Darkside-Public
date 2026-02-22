package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class AddMineCommand extends AddMapEntityCommand
   {
      private static var _instance:AddMineCommand;
      
      public var maxSkill5b:Boolean = false;
      
      public var maxSkill1:Boolean = false;
      
      public var typeId:int = 0;
      
      public function AddMineCommand(param1:String = "", param2:int = 0, param3:int = 0, param4:int = 0, param5:Boolean = false, param6:Boolean = false)
      {
         super(param1,param2,param3);
         this.maxSkill5b = param6;
         this.maxSkill1 = param5;
         this.typeId = param4;
      }
      
      public static function get instance() : AddMineCommand
      {
         return _instance || (_instance = new AddMineCommand());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 28996;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.maxSkill5b = param1.readBoolean();
         this.maxSkill1 = param1.readBoolean();
         this.typeId = param1.readInt();
         this.typeId = this.typeId >>> 16 | this.typeId << 16;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(28996);
         super.write(param1);
         param1.writeBoolean(this.maxSkill5b);
         param1.writeBoolean(this.maxSkill1);
         param1.writeInt(this.typeId << 16 | this.typeId >>> 16);
      }
   }
}

