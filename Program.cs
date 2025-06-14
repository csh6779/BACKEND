using RigidboysAPI.Data;
using RigidboysAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("🔧 연결 문자열: " + builder.Configuration.GetConnectionString("MySql"));

// 1️⃣ MySQL DB 연결
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"),
    new MySqlServerVersion(new Version(8, 0, 33))));

// 2️⃣ 서비스 등록 (DI)
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CustomerMutationService>();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductMutationService>();

builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<PurchaseMutationService>();

// ✅ ✅ ✅ CORS 정책 등록 추가
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3️⃣ 컨트롤러 + Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
     options.EnableAnnotations(); // ✅ Swagger 어노테이션 기능 활성화
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath); // 🔥 이 줄이 중요!
});

var app = builder.Build();

// ✅ ✅ ✅ CORS 미들웨어 추가 (반드시 HTTPS보다 위에!)
app.UseCors("AllowAll");

// 4️⃣ 개발환경에서 Swagger UI 사용
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // ⬅ 이 줄보다 위에 CORS가 있어야 함
app.UseAuthorization();
app.MapControllers();

app.Run();
