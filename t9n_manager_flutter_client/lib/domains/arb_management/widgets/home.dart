import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/generated/l10n.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';
import 'package:t9n_manager_flutter_client/shared/widgets/menu_card.dart';

class Home extends StatefulWidget {
  const Home({Key? key}) : super(key: key);

  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  @override
  Widget build(BuildContext context) {
    double width = MediaQuery.of(context).size.width > context.watch<AppState>().screenMaxSize ? context.watch<AppState>().screenMaxSize : MediaQuery.of(context).size.width;
    double cardSize = context.watch<AppState>().cardSize;
    int crossAxisCount = (width / cardSize).floor();
    var selectedProject = context.watch<AppState>().selectedProject;
    return Flexible(
      child: GridView.count(
        crossAxisCount: crossAxisCount,
        padding: const EdgeInsets.all(5.0),
        children: [
          MenuCard(
            cardSize: cardSize,
            title: S.of(context).menu_tenants,
            onPressed:()=> tapMenu(context,'/tenant'),
          ),    
          MenuCard(
            cardSize: cardSize,
            title: S.of(context).menu_projects,
            onPressed:()=> tapMenu(context,'/project'),
          ),
          if (selectedProject.name!="(none)")
          MenuCard(
            cardSize: cardSize, 
            title: S.of(context).menu_arb_management,
            onPressed: ()=>tapMenu(context, '/arb_management'),
          ),
    
        ],
      ),
    );
  }

  tapMenu(BuildContext context, String menu) => Navigator.pushNamed(context, menu);
}
