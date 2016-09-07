
$(document).ready(function () {
    $('#example').DataTable({
        "order": [[1, "asc"]],
        "lengthMenu": [ [10, 25, 50, -1], [10, 25, 50, "All"] ],
        "language": {
            "lengthMenu": "每页显示 _MENU_ 记录",
            "zeroRecords": "没有找到记录",
            "info": "显示第_PAGE_页 共 _PAGES_页",
            "infoEmpty": "无可用记录",
            "infoFiltered": "(filtered from _MAX_ total records)",
            "search": "搜索",
            "paginate": {
                "first": "首页",
                "last": "尾页",
                "next": "下一页",
                "previous": "上一页"
            },
        }
    });
});