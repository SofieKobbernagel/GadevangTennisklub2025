using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IMemberService, MemberService>();
builder.Services.AddTransient<ICourtService, CourtService>();
builder.Services.AddTransient<ICourtTypeService, CourtTypeService>();
builder.Services.AddTransient<IEventServiceAsync, EventServicesAsync>();
builder.Services.AddTransient<IRelationshipsServicesAsync, RelationshipsServicesAsync>();
builder.Services.AddTransient<IBookingServiceAsync, BookingServiceAsync>();
builder.Services.AddTransient<ITeamService, TeamService>();

builder.Services.AddSession();    //Nyt
builder.Services.AddHttpContextAccessor();//Nyt
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<ICoachService, CoachService>();
//builder.Services.AddScoped<ITeamService, TeamService>();





var app = builder.Build();


/// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
