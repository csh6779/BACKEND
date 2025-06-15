using RigidboysAPI.Data;
using RigidboysAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("🔧 연결 문자열: " + builder.Configuration.GetConnectionString("MySql"));

// 1️⃣ MySQL DB 연결
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"),
    new MySqlServerVersion(new Version(8, 0, 33))));

// 1.5️⃣ 로그인 설정 - JWT 인증 추가
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var config = builder.Configuration;
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();

// 2️⃣ 서비스 등록 (DI)
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CustomerMutationService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductMutationService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<PurchaseMutationService>();
builder.Services.AddScoped<UserService>();


// ✅ CORS 정책 등록
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

builder.Services.AddEndpointsApiExplorer(); // ✅ 하나만 유지

// ✅ Swagger에 JWT 인증 정의 추가
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // 🔐 JWT 인증 설정
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token} 형식으로 입력해주세요."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ✅ 미들웨어 순서 매우 중요!
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ 인증/인가 순서 지켜야 함!
app.UseAuthentication(); // 반드시 먼저 호출
app.UseAuthorization();

app.MapControllers();

app.Run();
