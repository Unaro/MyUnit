using MyUnit.Attributes;

namespace SystemArithmetic.Tests
{
    public class SumOperationTest
    {
        [MyFact]
        public void SumOnePlusOne_EqualsTwo()
        {
            MyAssert.Equal(2, 2);
        }

        [MyFact]
        public void SumTwo_EqualsThree()
        {
            MyAssert.Equal(3, 3);
        }

        [MyInlineData(1,5,6,7)]
        [MyInlineData(1, -5, 16, 7)]
        public void Sum(int one, int two, int three, int four)
        {
            MyAssert.Equal(19, one+two+three+four);
        }

        [MyInlineData(2, 0)]
        [MyInlineData(2, 5)]
        public void DivisionByZero(int num1, int num2)
        {
            MyAssert.Throws<DivideByZeroException>(() => { var num = num1 / num2; }) ;
        }

        [MyFact]
        public void IncorrectExeption()
        {
            var num1 = 9;
            MyAssert.Throws<DivideByZeroException>(() => { var num = num1 / 0; });
        }


        [MyFact]
        public void TwoIsMoreThanThree_False()
        {
            MyAssert.False(2 > 3);
        }

        [MyFact]
        public void TwoIsLittleThanThree_True()
        {
            MyAssert.True(4 < 4);
        }

        [MyFact]
        public void TwoAsLittleThanThree_True()
        {
            MyAssert.False(3 < 3);
        }

    }
}