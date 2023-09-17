using Math = System.Math;
namespace Zdog.Maui.Render
{
    public partial class Vector
    {
        public float x, y, z;

        public Vector(Vector vector) : this(vector.x, vector.y, vector.z)
        {
        }

        public Vector(float x = 0f, float y = 0f, float z = 0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector set(Vector vector)
        {
            if(vector!=null)
                set(vector.x, vector.y, vector.z);
            return this;
        }

        public Vector set(float x = 0f, float y = 0f, float z = 0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
        }

        public Vector rotate(Vector rotation)
        {
            rotateZ(rotation.z);
            rotateY(rotation.y);
            rotateX(rotation.x);
            return this;
        }

        public Vector rotateZ(float angle)
        {
            if (angle == 0f || angle % Utils.TAU == 0.0)
            {
                return this;
            }
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var tempA = x;
            var tempB = y;
            x = ((float)(tempA * cos - tempB * sin));
            y = ((float)(tempB * cos + tempA * sin));
            return this;
        }

        public Vector rotateY(float angle)
        {
            if (angle == 0f || angle % Utils.TAU == 0.0)
            {
                return this;
            }
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var tempA = x;
            var tempB = z;
            x = ((float)(tempA * cos - tempB * sin));
            z = ((float)(tempB * cos + tempA * sin));
            return this;
        }

        public Vector rotateX(float angle)
        {
            if (angle == 0f || angle % Utils.TAU == 0.0)
            {
                return this;
            }
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var tempA = y;
            var tempB = z;
            y = ((float)(tempA * cos - tempB * sin));
            z = ((float)(tempB * cos + tempA * sin));
            return this;
        }

        public bool isSame(Vector pos)
        {
            return x == pos.x && y == pos.y && z == pos.z;
        }

        public Vector add(Vector pos)
        {
            return add(pos.x, pos.y, pos.z);
        }

        public Vector add(float x = 0f, float y = 0f, float z = 0f)
        {
            this.x += x;
            this.y += y;
            this.z += z;
            return this;
        }

        public Vector subtract(Vector pos)
        {
            return subtract(pos.x, pos.y, pos.z);
        }

        Vector subtract(float x = 0f, float y = 0f, float z = 0f)
        {
            this.x -= x;
            this.y -= y;
            this.z -= z;
            return this;
        }

        public Vector multiply(Vector pos)
        {
            return multiply(pos.x, pos.y, pos.z);
        }

        public Vector multiply(float x = 1f, float y = 1f, float z = 1f)
        {
            this.x *= x;
            this.y *= y;
            this.z *= z;
            return this;
        }

        public Vector multiply(float pos)
        {
            x *= pos;
            y *= pos;
            z *= pos;
            return this;
        }

        public Vector transform(Vector translation, Vector rotation, Vector scale)
        {
            multiply(scale);
            rotate(rotation);
            add(translation);
            return this;
        }

        public void lerp(Vector pos, float alpha)
        {
            lerp(pos.x, pos.y, pos.z, alpha);
        }

        public Vector lerp(float x = 0f, float y = 0f, float z = 0f, float alpha = 0)
        {
            this.x = Utils.lerp(this.x, x, alpha);
            this.y = Utils.lerp(this.y, y, alpha);
            this.z = Utils.lerp(this.z, z, alpha);
            return this;
        }

        public float magnitude()
        {
            return Utils.magnitudeSqrt(x * x + y * y + z * z);
        }

        public float magnitude2d()
        {
            return Utils.magnitudeSqrt(x * x + y * y);
        }

        public string to2d()
        {
            return "($x, $y)";
        }

        public string to3d()
        {
            return "($x, $y, $z)";
        }

        internal Vector copy()
        {
            return new Vector(x, y, z);
        }
    }
}
