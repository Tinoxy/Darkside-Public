package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AttributeSkillShieldUpdateCommand implements IModule
   {
      private static var _instance:AttributeSkillShieldUpdateCommand;
      
      public var shieldSkillId:int = 0;
      
      public var minSkinShieldTwinkle:int = 0;
      
      public var maxSkinShieldTwinkle:int = 0;
      
      public function AttributeSkillShieldUpdateCommand(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.shieldSkillId = param1;
         this.minSkinShieldTwinkle = param2;
         this.maxSkinShieldTwinkle = param3;
      }
      
      public static function get instance() : AttributeSkillShieldUpdateCommand
      {
         return _instance || (_instance = new AttributeSkillShieldUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 103;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.shieldSkillId = param1.readInt();
         this.shieldSkillId = this.shieldSkillId << 6 | this.shieldSkillId >>> 26;
         this.minSkinShieldTwinkle = param1.readInt();
         this.minSkinShieldTwinkle = this.minSkinShieldTwinkle >>> 13 | this.minSkinShieldTwinkle << 19;
         this.maxSkinShieldTwinkle = param1.readInt();
         this.maxSkinShieldTwinkle = this.maxSkinShieldTwinkle >>> 2 | this.maxSkinShieldTwinkle << 30;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(103);
         param1.writeInt(this.shieldSkillId >>> 6 | this.shieldSkillId << 26);
         param1.writeInt(this.minSkinShieldTwinkle << 13 | this.minSkinShieldTwinkle >>> 19);
         param1.writeInt(this.maxSkinShieldTwinkle << 2 | this.maxSkinShieldTwinkle >>> 30);
      }
   }
}

