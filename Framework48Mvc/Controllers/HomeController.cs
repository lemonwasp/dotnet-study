using Framework48Mvc.Services;
using System.Web.Mvc;

namespace Framework48Mvc.Controllers
{
    // Syste.Web.MvcのControllerクラスを継承することで、
    // MVCのコントローラーとして機能する
    public class HomeController : Controller
    {
        // フィールドが生成された後、他のオブジェクトに変わらないよう制限
        // ただし、readonlyはオブジェクト内部データの変更まで防止するのではない
        // また、privateフィールド前に_を付けるのは、C#のコーディング規約で推奨されている
        //private readonly HomeService _homeService = new HomeService();
        // ActionResultは、MVCのアクションメソッドが返す型であり、HTTPレスポンスを表す
        // IndexはHomeControllerのデフォルトアクションメソッドであり、
        // /や/Home/や/Home/Indexにアクセスした際に呼び出される
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        public ActionResult Index()
        {
            // ControllerとActionの名前を組み合わせたViewを返すことで、
            // MVCのビューエンジンが自動的に対応するビューを探してレンダリングする
            // Views/Home/Index.cshtml
            return View();
        }

        // このメソッドは、HTTP GETリクエストだけを受け付けることを示す属性であり、
        // ブラウザからのアクセスやAjaxリクエストなどで呼び出される
        [HttpGet]
        // ActionResultの派生クラスであるJsonResultを返すことで、
        // JSON形式のデータを返すことができる
        public JsonResult GetMessages()
        {
            // varは右の値を推論して型を決定するキーワードであり、
            // ここではHomeServiceのGetMessageメソッドが返すオブジェクトを受け取る
            // コンパイル時に型が決定されるため、実行時のパフォーマンスに影響しない
            // つまり、varは動的型ではなく、静的型であるため、型安全性が保たれる
            var result = _homeService.GetMessages();

            // C#オブジェクトをJSON形式にシリアライズして返すためのメソッド
            return Json(
                result,
                // JsonRequestBehaviorは、JSONリクエストの挙動を制御する列挙型であり、
                // AllowGetはHTTP GETリクエストを許可する
                JsonRequestBehavior.AllowGet
            );
        }

        public ActionResult About()
        {
            // ViewBagは、ControllerとView間でデータを渡すための動的プロパティ
            // ただし、コンパイル時に型チェックが行われないため、
            // 型安全性が低いので、規模が大きくなる場合はViewModelを使用することが推奨される
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}